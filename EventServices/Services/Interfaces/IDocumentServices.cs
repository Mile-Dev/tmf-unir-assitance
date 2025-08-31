using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Dto.Query;
using SharedServices.Objects;

namespace EventServices.Services.Interfaces
{
    /// <summary>
    /// Define los métodos para interactuar con servicios de almacenamiento S3,
    /// permitiendo la generación de URLs prefirmadas para subir y descargar archivos.
    /// </summary>
    public interface IDocumentServices
    {
        /// <summary>
        /// Genera una URL prefirmada para subir un archivo a S3.
        /// </summary>
        /// <param name="filename">Nombre del archivo a subir.</param>
        /// <param name="eventNumber">Identificador del evento asociado al archivo.</param>
        /// <param name="contentType">Tipo de contenido del archivo.</param>
        /// <returns>URL prefirmada para la subida del archivo.</returns>
        Task<string> GetPresignedUploadUrlAsync(int eventNumber, DocumentUploadDto documentDownloadDto);

        /// <summary>
        /// Genera una URL prefirmada para descargar un archivo desde S3.
        /// </summary>
        /// <param name="eventNumber">Identificador del evento asociado al archivo.</param>
        /// <param name="filename">Nombre del archivo a descargar.</param>
        /// <returns>URL prefirmada para la descarga del archivo.</returns>
        Task<string> GetPresignedDownloadUrlAsync(int eventNumber, DocumentDownloadDto documentDownloadDto);

        /// <summary>
        /// Genera una URL prefirmada para descargar un archivo desde S3.
        /// </summary>
        /// <param name="eventNumber">Identificador del evento asociado al archivo.</param>
        /// <param name="filename">Nombre del archivo a descargar.</param>
        /// <returns>URL prefirmada para la descarga del archivo.</returns>
        Task<ResponseCreatedDto> CreatedDocumentAsync(DocumentCreatedDto documentCreatedDto);

        /// <summary>
        /// Obtiene una lista paginada de documentos asociados a un evento específico.
        /// </summary>
        /// <param name="id">Identificador del evento para filtrar los documentos.</param>
        /// <param name="parameterGetList">Identificador del evento para filtrar los documentos.</param>
        /// <returns>Datos paginados que contienen la lista de documentos del evento y el total de registros.</returns>
        Task<PaginatedDataQueryDto> GetListDocumentByEventIdAsync(int id, ParameterGetList parameterGetList);
    }
}
