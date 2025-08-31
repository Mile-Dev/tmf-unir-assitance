using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio específico para la entidad CustomerTrip.
    /// Hereda de Repository<CustomerTrip> y expone operaciones de acceso a datos para CustomerTrip.
    /// </summary>
    public class CustomerTripRepository(MainContext context) : Repository<CustomerTrip>(context), ICustomerTripRepository
    {
        // No se agregan métodos adicionales, se utilizan los métodos heredados de Repository<CustomerTrip>:
        // - Remove(T entity): Elimina una entidad CustomerTrip.
        // - Update(T entity): Actualiza una entidad CustomerTrip.
        // - GetAll(List<Expression<Func<T, bool>>>? filters = null): Obtiene todas las entidades CustomerTrip, opcionalmente filtradas.
        // - AddRangeAsync(IEnumerable<T> entities): Agrega varias entidades CustomerTrip de forma asíncrona.
        // - UpdateRangeAsync(IEnumerable<T> entities): Actualiza varias entidades CustomerTrip de forma asíncrona.
        // - DeleteRangeAsync(IEnumerable<T> entities): Elimina varias entidades CustomerTrip de forma asíncrona.
    }
}
