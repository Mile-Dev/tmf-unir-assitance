using EventServices.Domain.Entities;
using EventServices.Domain.Projections;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    /// <summary>
    /// Interfaz para el repositorio de pagos de garantía.
    /// Define operaciones específicas para la entidad GuaranteePayment, además de las operaciones genéricas del repositorio.
    /// </summary>
    public interface IGuaranteePaymentRepository : IRepository<GuaranteePayment>
    {
        /// <summary>
        /// Obtiene un pago de garantía por su identificador único.
        /// </summary>
        /// <param name="id">Identificador del pago de garantía.</param>
        /// <returns>La entidad GuaranteePayment encontrada o null si no existe.</returns>
        Task<GuaranteePayment?> GetGuaranteePaymentById(int id);

        /// <summary>
        /// Obtiene una proyección de log de evento por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="id">Identificador del log de evento.</param>
        /// <returns>La proyección EventLogProjection encontrada o null si no existe.</returns>
        Task<EventLogProjection?> GetEventLogProjectionByGuaranteeIdAsync(int id);
    }
}
