using EventServices.Domain.Entities;
using EventServices.Domain.Projections;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    /// <summary>
    /// Interfaz para el repositorio de estados de proveedores de eventos.
    /// Hereda de <see cref="IRepository{EventProviderStatus}"/> y define métodos específicos para la obtención de identificadores y códigos de estado.
    /// </summary>
    public interface IEventProviderStatusRepository : IRepository<EventProviderStatus>
    {
        /// <summary>
        /// Obtiene de forma asíncrona el identificador del estado a partir de su código.
        /// </summary>
        /// <param name="code">Código del estado.</param>
        /// <returns>Identificador del estado correspondiente al código proporcionado.</returns>
        Task<int> GetStatusIdByCodeAsync(string code);

        /// <summary>
        /// Obtiene de forma asíncrona una lista de objetos <see cref="StatusCodeDto"/> para los códigos de estado proporcionados.
        /// </summary>
        /// <param name="codes">Colección de códigos de estado.</param>
        /// <returns>Lista de DTOs con identificadores y códigos de estado.</returns>
        Task<List<StatusCodeDto>> GetManyByCodesAsync(IEnumerable<string> codes);
    }
}
