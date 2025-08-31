using EventServices.Domain.Entities;
using EventServices.Domain.Projections;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    /// <summary>
    /// Define la interfaz para el repositorio de eventos, extendiendo las operaciones genéricas de <see cref="IRepository{Event}"/>.
    /// Proporciona métodos adicionales para obtener agregados y proyecciones específicas de eventos.
    /// </summary>
    public interface IEventsRepository : IRepository<Event>
    {
        /// <summary>
        /// Obtiene de forma asíncrona el agregado completo de un evento por su identificador.
        /// Incluye las relaciones y entidades asociadas necesarias para el dominio.
        /// </summary>
        /// <param name="id">Identificador único del evento.</param>
        /// <returns>El evento con sus datos agregados o null si no existe.</returns>
        Task<Event?> GetEventAggregateAsync(int id);

        /// <summary>
        /// Obtiene de forma asíncrona una proyección de log del evento por su identificador.
        /// Esta proyección está optimizada para operaciones de registro o auditoría.
        /// </summary>
        /// <param name="id">Identificador único del evento.</param>
        /// <returns>La proyección de log del evento o null si no existe.</returns>
        Task<EventLogProjection?> GetEventLogProjectionByIdAsync(int id);
    }
}
