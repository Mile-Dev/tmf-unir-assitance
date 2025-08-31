using MasterRdsServices.Domain.Entities;
using MasterRdsServices.Infraestructura.DataAccess.Common;
using MasterRdsServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using MasterRdsServices.Infraestructura.DataAccess.Repository;

namespace MasterRdsServices.Infraestructura.DataAccess.Dao
{
    public class EventProviderStatusRepository(MainContext context) : Repository<EventProviderStatus>(context), IEventProviderStatusRepository
    {
    }
}
