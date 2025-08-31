using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la entidad ContactEmergency.
    /// Hereda de Repository<ContactEmergency> y expone operaciones específicas para contactos de emergencia.
    /// </summary>
    public class ContactEmergencyRepository(MainContext context) : Repository<ContactEmergency>(context), IContactEmergencyRepository
    {
        // No se agregan métodos adicionales, se utilizan los métodos genéricos del repositorio base.
        // Métodos heredados:
        // - IQueryable<ContactEmergency> GetAll(List<Expression<Func<ContactEmergency, bool>>>? filters = null)
        // - Task AddRangeAsync(IEnumerable<ContactEmergency> entities)
        // - void Remove(ContactEmergency entity)
        // - void Update(ContactEmergency entity)
        // - Task UpdateRangeAsync(IEnumerable<ContactEmergency> entities)
        // - Task DeleteRangeAsync(IEnumerable<ContactEmergency> entities)
    }
}
