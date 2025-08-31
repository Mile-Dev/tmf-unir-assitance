using EventStatusSwitchTempServices.Domain.Entities;
using EventStatusSwitchTempServices.Infraestructura.DataAccess.Repository;
using EventStatusSwitchTempServices.Infraestructura.Interface.EntitiesDao;

namespace EventStatusSwitchTempServices.Infraestructura.DataAccess.Dao
{
    public class EventsRepository(MainContext context) : Repository<Events>(context), IEventsRepository
    {
    }
}
