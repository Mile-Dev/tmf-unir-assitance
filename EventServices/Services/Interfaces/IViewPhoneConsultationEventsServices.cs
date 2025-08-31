using EventServices.Domain.Dto.Query;
using SharedServices.Objects;

namespace EventServices.Services.Interfaces
{
    /// <summary>
    /// Interfaz para los servicios relacionados con la consulta paginada de eventos de consulta telefónica.
    /// </summary>
    public interface IViewPhoneConsultationEventsServices
    {
        /// <summary>
        /// Obtiene una lista paginada de eventos según los filtros proporcionados.
        /// </summary>
        /// <param name="filters">Filtros y parámetros de paginación a aplicar en la consulta.</param>
        /// <returns>Un objeto <see cref="PaginatedDataQueryDto"/> que contiene los datos paginados y el total de registros.</returns>
        Task<PaginatedDataQueryDto> GetEventPaginatedAsync(Filters filters);
    }
}
