using EventServices.Domain.Entities;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    /// <summary>
    /// Define la interfaz para el repositorio de estados de eventos.
    /// Hereda de <see cref="IRepository{EventStatus}"/> y agrega operaciones específicas para la entidad <see cref="EventStatus"/>.
    /// </summary>
    public interface IEventStatusRepository : IRepository<EventStatus>
    {
        /// <summary>
        /// Obtiene de forma asíncrona el identificador del estado de evento a partir de su código.
        /// </summary>
        /// <param name="code">Código único del estado de evento.</param>
        /// <returns>Identificador del estado de evento correspondiente al código proporcionado.</returns>
        Task<int> GetStatusIdByCodeAsync(string code);
    }
}
