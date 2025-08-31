using EventServices.Domain.Entities;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    public interface ICategoriesRepository : IRepository<Category>
    {
        Task<bool> GetByName(string name);

    }
}
