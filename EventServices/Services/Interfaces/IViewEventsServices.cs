using EventServices.Domain.Dto.Query;
using SharedServices.Objects;

namespace EventServices.Services.Interfaces
{
    /// <summary>
    /// Interfaz para los servicios de visualización de eventos.
    /// Proporciona métodos para obtener eventos paginados según filtros específicos.
    /// </summary>
    public interface IViewEventsServices
    {
        /// <summary>
        /// Obtiene una lista paginada de eventos según los filtros proporcionados.
        /// </summary>
        /// <param name="filters">Filtros y parámetros de paginación para la consulta.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica. 
        /// El resultado contiene un objeto <see cref="PaginatedDataQueryDto"/> con los datos paginados y el total de registros.
        /// </returns>
        Task<PaginatedDataQueryDto> GetEventPaginatedAsync(Filters filters);
    }
}
