using TrackingMokServices.Infraestructura.DataAccess.Common;
using TrackingMokServices.Infraestructura.DataAccess.Dao;
using TrackingMokServices.Infraestructura.DataAccess.Interface;
using TrackingMokServices.Infraestructura.DataAccess.Interface.EntitiesDao;

namespace TrackingMokServices.Infraestructura.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MainContext _context;

        public UnitOfWork(MainContext context)
        {
            _context = context;
            ViewSummaryEventMokRepository = new ViewSummaryEventMokRepository(_context);
        }


        public IViewSummaryEventMokRepository ViewSummaryEventMokRepository { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
