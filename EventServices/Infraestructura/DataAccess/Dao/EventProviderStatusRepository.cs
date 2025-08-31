using EventServices.Domain.Entities;
using EventServices.Domain.Projections;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la entidad EventProviderStatus.
    /// Proporciona métodos específicos para consultar estados de proveedores de eventos.
    /// </summary>
    public class EventProviderStatusRepository(MainContext context) : Repository<EventProviderStatus>(context), IEventProviderStatusRepository
    {
        /// <summary>
        /// Obtiene el identificador del estado a partir de su código.
        /// </summary>
        /// <param name="code">Código del estado.</param>
        /// <returns>Identificador del estado si existe, o 0 si no se encuentra.</returns>
        public async Task<int> GetStatusIdByCodeAsync(string code)
        {
            return await Entities
                .Where(status => status.Code == code)
                .Select(status => status.Id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene una lista de estados a partir de una colección de códigos.
        /// </summary>
        /// <param name="codes">Colección de códigos de estado.</param>
        /// <returns>Lista de objetos StatusCodeDto con los identificadores y códigos encontrados.</returns>
        public async Task<List<StatusCodeDto>> GetManyByCodesAsync(IEnumerable<string> codes)
        {
            return await Entities
                .Where(status => codes.Contains(status.Code))
                .Select(status => new StatusCodeDto
                {
                    Id = status.Id,
                    Code = status.Code
                })
                .ToListAsync();
        }
    }
}
