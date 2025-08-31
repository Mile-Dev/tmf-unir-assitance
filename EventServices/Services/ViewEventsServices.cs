using AutoMapper;
using EventServices.Domain.Dto.Query;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Services.Interfaces;
using SharedServices.Objects;

namespace EventServices.Services
{
    /// <summary>
    /// Servicio para la obtención paginada de eventos desde la vista de eventos.
    /// Utiliza un UnitOfWork para acceder a los repositorios, un logger para el registro de errores y AutoMapper para la conversión de entidades a DTOs.
    /// </summary>
    public class ViewEventsServices(IUnitOfWork unitOfWork, ILogger<ViewEventsServices> logger, IMapper mapper) : IViewEventsServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<ViewEventsServices> _logger = logger;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Obtiene una lista paginada de eventos aplicando filtros, ordenamiento y paginación.
        /// </summary>
        /// <param name="filters">Filtros y parámetros de paginación/ordenamiento.</param>
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

                var viewPaginatedevents = await _unitOfWork.ViewEventsRepository.GetPaginatedData(pageNumberValue, pageSizeValue, filters.Filter, SortBy, SortOrder, cancellationToken);

                var getAllvieweventss = _mapper.Map<List<ViewEventsGetDto>>(viewPaginatedevents.Data);

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
