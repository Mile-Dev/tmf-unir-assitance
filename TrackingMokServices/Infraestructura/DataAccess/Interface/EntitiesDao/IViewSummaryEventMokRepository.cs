using TrackingMokServices.Domain.Entities;

namespace TrackingMokServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    public interface IViewSummaryEventMokRepository : IRepository<EventMok>
    {
        Task<List<EventMok>> ListEventMokAsync(string id);
    }
}
