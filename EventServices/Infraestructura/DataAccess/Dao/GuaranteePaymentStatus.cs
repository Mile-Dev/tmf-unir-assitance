using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio específico para la entidad GuaranteePaymentStatus.
    /// Hereda de Repository<GuaranteePaymentStatus> y expone las operaciones de acceso a datos
    /// relacionadas con el estado de los pagos de garantía.
    /// </summary>
    public class GuaranteePaymentStatusRepository(MainContext context)
        : Repository<GuaranteePaymentStatus>(context), IGuaranteePaymentStatusRepository
    {
        // Actualmente no se han definido métodos adicionales.
        // Se utilizan los métodos genéricos proporcionados por la clase base Repository<T>.
    }
}
