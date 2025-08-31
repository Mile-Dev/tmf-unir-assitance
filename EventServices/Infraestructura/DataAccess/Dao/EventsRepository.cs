using EventServices.Domain.Entities;
using EventServices.Domain.Projections;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para operaciones específicas sobre la entidad Event.
    /// </summary>
    public class EventsRepository(MainContext context) : Repository<Event>(context), IEventsRepository
    {
        /// <summary>
        /// Obtiene un evento por su identificador, incluyendo sus relaciones principales.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Entidad Event con sus relaciones cargadas o null si no existe.</returns>
        public async Task<Event?> GetEventAggregateAsync(int id)
        {
            return await Entities
                       .Include(a => a.EventStatusNavigation)
                       .Include(a => a.GeneralTypesNavigation)
                       .Include(a => a.VoucherNavigation)
                            .ThenInclude(v => v!.Client)
                       .Include(a => a.CustomerTripNavigation)
                       .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Obtiene una proyección de log de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Proyección EventLogProjection o null si no existe.</returns>
        public async Task<EventLogProjection?> GetEventLogProjectionByIdAsync(int id)
        {
            return await Entities
                .Where(e => e.Id == id)
                .Select(e => new EventLogProjection
                {
                    Id = e.Id,
                    VoucherName = e.VoucherNavigation!.Name,
                    ClientCode = e.VoucherNavigation != null && e.VoucherNavigation.Client != null ? e.VoucherNavigation.Client.Code : null,
                    ClientId = e.VoucherNavigation != null && e.VoucherNavigation.Client != null ? e.VoucherNavigation.Client.Id : null,
                    EventStatusName = e.EventStatusNavigation != null ? e.EventStatusNavigation.Name : null,
                    EventStatusId = e.EventStatusId
                })
                .FirstOrDefaultAsync();
        }
    }
}
