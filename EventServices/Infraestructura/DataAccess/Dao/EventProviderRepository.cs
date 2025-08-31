using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la entidad EventProvider, encargado de la gestión de proveedores de eventos en la base de datos.
    /// Hereda de Repository<EventProvider> y expone métodos específicos para la consulta de proveedores de eventos.
    /// </summary>
    public class EventProviderRepository(MainContext context) : Repository<EventProvider>(context), IEventProviderRepository
    {
        /// <summary>
        /// Verifica de forma asíncrona si existe un proveedor de evento con el identificador de solicitud externa especificado.
        /// </summary>
        /// <param name="id">Identificador de la solicitud externa.</param>
        /// <returns>True si existe un proveedor con ese identificador, de lo contrario false.</returns>
        public async Task<bool> ExistsEventProviderByExternalRequestIdAsync(string id)
          => await Entities
        .AnyAsync(item => item.ExternalRequestId == id);

        /// <summary>
        /// Obtiene de forma asíncrona la lista de proveedores de evento asociados a un evento específico.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Lista de proveedores de evento relacionados con el evento indicado.</returns>
        public async Task<List<EventProvider>> GetEventProviderByEventIdAsync(int id)
         => await Entities
        .Include(a => a.EventProviderNavigation)
        .Where(x => x.EventId == id).ToListAsync();

        /// <summary>
        /// Obtiene de forma asíncrona un proveedor de evento por su identificador único.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>Proveedor de evento encontrado o null si no existe.</returns>
        public async Task<EventProvider?> GetEventProviderByIdAsync(int id)
        => await Entities
        .Include(a => a.EventProviderNavigation)
        .FirstOrDefaultAsync(x => x.Id == id);
    }
}
