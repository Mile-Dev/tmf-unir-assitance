using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la entidad <see cref="Client"/> que implementa operaciones específicas de acceso a datos.
    /// Hereda de <see cref="Repository{Client}"/> y utiliza el contexto <see cref="MainContext"/>.
    /// </summary>
    public class ClientRepository(MainContext context) : Repository<Client>(context), IClientRepository
    {
        /// <summary>
        /// Obtiene un cliente por su nombre de forma asíncrona.
        /// </summary>
        /// <param name="name">Nombre del cliente a buscar.</param>
        /// <returns>Instancia de <see cref="Client"/> si existe, o null en caso contrario.</returns>
        public async Task<Client?> GetByNameAsync(string name)
            => await Entities.FirstOrDefaultAsync(x => x.Name == name);
    }
}
