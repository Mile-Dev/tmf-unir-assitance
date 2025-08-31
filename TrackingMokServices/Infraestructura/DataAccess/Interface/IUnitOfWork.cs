using TrackingMokServices.Infraestructura.DataAccess.Interface.EntitiesDao;

namespace TrackingMokServices.Infraestructura.DataAccess.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IViewSummaryEventMokRepository ViewSummaryEventMokRepository { get; }

        Task<int> CompleteAsync();
    }
}
