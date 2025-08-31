using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la entidad <see cref="EventStatus"/>.
    /// Proporciona operaciones específicas para la obtención de estados de eventos.
    /// Hereda de <see cref="Repository{EventStatus}"/> e implementa <see cref="IEventStatusRepository"/>.
    /// </summary>
    public class EventStatusRepository(MainContext context) : Repository<EventStatus>(context), IEventStatusRepository
    {
        /// <summary>
        /// Obtiene de forma asíncrona el identificador del estado de evento a partir de su código.
        /// </summary>
        /// <param name="code">Código único del estado de evento.</param>
        /// <returns>Identificador del estado de evento correspondiente al código proporcionado, o 0 si no existe.</returns>
        public async Task<int> GetStatusIdByCodeAsync(string code)
        {
            return await Entities
                .Where(status => status.Code == code)
                .Select(status => status.Id)
                .FirstOrDefaultAsync();
        }
    }
}
