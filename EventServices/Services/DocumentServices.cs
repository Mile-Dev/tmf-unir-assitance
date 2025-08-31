using AutoMapper;
using EventServices.Common;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Dto.Query;
using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Services.Interfaces;
using SharedServices.Objects;
using StorageS3Services.Common.Interfaces;

namespace EventServices.Services
{
    /// <summary>
    /// Servicio para la gestión de documentos asociados a eventos, incluyendo la generación de URLs prefirmadas para S3 y la creación de registros de documentos.
    /// </summary>
    public class DocumentServices(IUnitOfWork unitOfWork, IMapper mapper, IS3Service s3Service, IConfiguration configuration, ILogger<DocumentServices> logger) : IDocumentServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public IS3Service _s3Service = s3Service;
        private readonly string _bucketName = configuration["AWS:S3:BucketName"]
                   ?? throw new ArgumentNullException("BucketName is not configured");
        private readonly ILogger<DocumentServices> _logger = logger;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Genera una URL prefirmada para subir un archivo a S3.
        /// </summary>
        /// <param name="eventNumber">Identificador del evento asociado al archivo.</param>
        /// <param name="documentDownloadDto">Tipo de contenido del archivo.</param>
        /// <returns>URL prefirmada para la subida del archivo.</returns>
        public async Task<string> GetPresignedUploadUrlAsync(int eventNumber, DocumentUploadDto documentDownloadDto)
        {

            string fileKey = $"{documentDownloadDto.Path}/{documentDownloadDto.FileName}";
            _logger.LogInformation("Generando URL prefirmada para subir el archivo: {Key} con tipo de contenido: {ContentType} en el bucket: {BucketName}", fileKey, documentDownloadDto.ContentType, _bucketName);
            var contentTypeDecode = Uri.UnescapeDataString(documentDownloadDto.ContentType);
            string presignedUrl = await _s3Service.GetPresignedUploadUrlAsync(fileKey, _bucketName, contentTypeDecode);
            _logger.LogInformation("La URL prefirmada para subir el archivo es: {PresignedUrl}", presignedUrl);
            return presignedUrl;
        }

        /// <summary>
        /// Genera una URL prefirmada para descargar un archivo desde S3.
        /// </summary>
        /// <param name="eventNumber">Identificador del evento asociado al archivo.</param>
        /// <returns>URL prefirmada para la descarga del archivo.</returns>
        public async Task<string> GetPresignedDownloadUrlAsync(int eventNumber, DocumentDownloadDto documentDownloadDto)
        {
            var fileKey = $"{documentDownloadDto.Path}/{documentDownloadDto.FileName}";
            string presignedUrl = await _s3Service.GetPresignedDownloadUrlAsync(fileKey, _bucketName);
            _logger.LogInformation("Descarga el archivo usando esta URL: {PresignedUrl}", presignedUrl);
            return presignedUrl;
        }

        /// <summary>
        /// Crea un registro de documento asociado a un evento.
        /// </summary>
        /// <param name="documentCreatedDto">DTO con la información del documento a crear.</param>
        /// <returns>DTO con la información del documento creado y el código del evento.</returns>
        /// <exception cref="ArgumentException">Se lanza si el evento asociado no existe.</exception>
        public async Task<ResponseCreatedDto> CreatedDocumentAsync(DocumentCreatedDto documentCreatedDto)
        {
            var eventObject = await _unitOfWork.EventsRepository.GetEventLogProjectionByIdAsync(documentCreatedDto.EventId);
            if (eventObject == null)
            {
                _logger.LogError("Evento con ID {EventId} no encontrado.", documentCreatedDto.EventId);
                throw new ArgumentException($"Evento con ID {documentCreatedDto.EventId} no encontrado.");
            }

            var documentObject = _mapper.Map<Document>(documentCreatedDto);
            var documentrecord = await _unitOfWork.DocumentsRepository.AddAsync(documentObject);
            await _unitOfWork.CompleteAsync();

            var EventCode = eventObject?.ClientCode + "-" + eventObject?.Id.ToString();

            ResponseCreatedDto responseCreatedDto = new()
            {
                Id = documentrecord.Id,
                CodeEvent = EventCode
            };

            return responseCreatedDto;
        }

        /// <summary>
        /// Obtiene una lista paginada de documentos asociados a un evento específico.
        /// </summary>
        /// <param name="id">Identificador del evento para filtrar los documentos.</param>
        /// <returns>Datos paginados que contienen la lista de documentos del evento y el total de registros.</returns>

        public async Task<PaginatedDataQueryDto> GetListDocumentByEventIdAsync(int id, ParameterGetList parameterGetList)
        {
            int pageSizeValue = parameterGetList.PageSize;
            int pageNumberValue = parameterGetList.PageNumber;
            string SortBy = parameterGetList.SortBy ?? "Id";
            string SortOrder = parameterGetList.SortOrder ?? "desc";
            var cancellationToken = new CancellationToken();

            Filters filters = new()
            {
                ParameterGetList = parameterGetList,
                Filter =
                [
                  new()
                    {
                      PropertyName = "EventId",
                      Value = id,
                      Comparison = 0
                    }
                ],
            };

            var documentsList = await _unitOfWork.DocumentsRepository.GetPaginatedData(pageNumberValue, pageSizeValue, filters.Filter, SortBy, SortOrder, cancellationToken);
            var documentsDtoList = _mapper.Map<List<DocumentGetDto>>(documentsList.Data);
            PaginatedDataQueryDto paginatedDataQueryDto = new(documentsDtoList, documentsList.TotalCount);
            return paginatedDataQueryDto;
        }
    }
}
