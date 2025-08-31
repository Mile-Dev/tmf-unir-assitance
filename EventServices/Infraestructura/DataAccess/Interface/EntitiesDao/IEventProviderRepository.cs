using EventServices.Domain.Entities;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    public interface IEventProviderRepository : IRepository<EventProvider>
    {
        Task<EventProvider?> GetEventProviderByIdAsync(int id);

        Task<List<EventProvider>> GetEventProviderByEventIdAsync(int id);

        Task<bool> ExistsEventProviderByExternalRequestIdAsync(string id);

    }
}
