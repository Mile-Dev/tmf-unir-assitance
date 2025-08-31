using EventStatusSwitchTempServices.Domain.Entities;
using EventStatusSwitchTempServices.Infraestructura.DataAccess.Repository;
using EventStatusSwitchTempServices.Infraestructura.Interface.EntitiesDao;

namespace EventStatusSwitchTempServices.Infraestructura.DataAccess.Dao
{
    public class EventStatusRepository(MainContext context) : Repository<EventStatus>(context), IEventStatusRepository
    {
    }
}
