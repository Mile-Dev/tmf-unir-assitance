using Microsoft.EntityFrameworkCore;
using TrackingMokServices.Domain.Entities;
using TrackingMokServices.Infraestructura.DataAccess.Common;
using TrackingMokServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using TrackingMokServices.Infraestructura.DataAccess.Repository;

namespace TrackingMokServices.Infraestructura.DataAccess.Dao
{
    public class ViewSummaryEventMokRepository(MainContext context) : Repository<EventMok>(context), IViewSummaryEventMokRepository
    {
        public async Task<List<EventMok>> ListEventMokAsync(string id)
       => await Entities.Where(x => x.EventoId == id).ToListAsync();
    }
}
