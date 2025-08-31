using EventServices.Domain.Entities;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client?> GetByNameAsync(string name);

    }
}
