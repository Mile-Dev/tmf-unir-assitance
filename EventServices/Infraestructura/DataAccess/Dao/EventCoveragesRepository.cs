using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio específico para la entidad EventCoverage.
    /// Hereda de Repository<EventCoverage> y expone las operaciones definidas en IEventCoveragesRepository.
    /// </summary>
    public class EventCoveragesRepository(MainContext context) : Repository<EventCoverage>(context), IEventCoveragesRepository
    {
        // Actualmente no se agregan métodos adicionales.
        // Hereda los métodos CRUD básicos de Repository<EventCoverage>.
    }
}
