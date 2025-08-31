using AutoMapper;
using EventServices.Domain.Dto.Query;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Services.Interfaces;
using SharedServices.Objects;

namespace EventServices.Services
{
    /// <summary>
    /// Servicio para la gestión de eventos de consulta telefónica.
    /// Implementa la interfaz <see cref="IViewPhoneConsultationEventsServices"/> y utiliza inyección de dependencias para acceder a la unidad de trabajo, el logger y el mapper.
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo para acceso a repositorios.</param>
    /// <param name="logger">Logger para el registro de errores y eventos.</param>
    /// <param name="mapper">Mapper para la conversión de entidades a DTOs.</param>
    public class ViewPhoneConsultationEventsServices(IUnitOfWork unitOfWork, ILogger<ViewPhoneConsultationEventsServices> logger, IMapper mapper) : IViewPhoneConsultationEventsServices
    {
        /// <summary>
        /// Unidad de trabajo para acceder a los repositorios de datos.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Logger para el registro de información y errores en el servicio.
        /// </summary>
        private readonly ILogger<ViewPhoneConsultationEventsServices> _logger = logger;

        /// <summary>
        /// Mapeador para la conversión de entidades a DTOs y viceversa.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Obtiene de forma paginada los eventos de consulta telefónica según los filtros proporcionados.
        /// </summary>
        /// <param name="filters">Filtros y parámetros de paginación.</param>
        /// <returns>Un objeto <see cref="PaginatedDataQueryDto"/> con la lista de eventos y el total de registros.</returns>
        public async Task<PaginatedDataQueryDto> GetEventPaginatedAsync(Filters filters)
        {
            try
            {
                int pageSizeValue = filters.ParameterGetList.PageSize;
                int pageNumberValue = filters.ParameterGetList.PageNumber;
                string SortBy = filters.ParameterGetList.SortBy ?? "Id";
                string SortOrder = filters.ParameterGetList.SortOrder ?? "desc";
                var cancellationToken = new CancellationToken();
                var viewPaginatedevents = await _unitOfWork.ViewPhoneConsultationEventsRepository.GetPaginatedData(pageNumberValue, pageSizeValue, filters.Filter, SortBy, SortOrder, cancellationToken);
                var getAllvieweventss = _mapper.Map<List<ViewPhoneConsultationEventGetDto>>(viewPaginatedevents.Data);
                PaginatedDataQueryDto paginatedDataQueryDto = new(getAllvieweventss, viewPaginatedevents.TotalCount);
                return paginatedDataQueryDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while geting the viewevents: {Message}", ex.Message);
                throw;
            }
        }
    }
}
