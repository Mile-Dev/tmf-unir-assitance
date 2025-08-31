using EventServices.Domain.Entities;
using EventServices.Domain.Projections;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la gestión de pagos de garantía.
    /// Implementa operaciones específicas para la entidad GuaranteePayment.
    /// </summary>
    public class GuaranteePaymentRepository(MainContext context) : Repository<GuaranteePayment>(context), IGuaranteePaymentRepository
    {
        /// <summary>
        /// Obtiene un pago de garantía por su identificador, incluyendo el estado y la navegación al proveedor del evento.
        /// </summary>
        /// <param name="id">Identificador del pago de garantía.</param>
        /// <returns>Entidad GuaranteePayment encontrada o null si no existe.</returns>
        public async Task<GuaranteePayment?> GetGuaranteePaymentById(int id)
            => await Entities
                    .Include(a => a.GuaranteePaymentStatus)
                    .Include(a => a.EventProviderNavigation)
                    .FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// Obtiene una proyección de log de evento por el identificador del pago de garantía.
        /// Incluye información del voucher, cliente y estado del evento relacionado.
        /// </summary>
        /// <param name="id">Identificador del pago de garantía.</param>
        /// <returns>Proyección EventLogProjection encontrada o null si no existe.</returns>
        public async Task<EventLogProjection?> GetEventLogProjectionByGuaranteeIdAsync(int id)
        {
            return await Entities
                .Where(guarantee => guarantee.Id == id)
                .Select(guarantee => new EventLogProjection
                {
                    Id = guarantee.EventProviderNavigation!.Id,
                    VoucherName = guarantee.EventProviderNavigation.Event.VoucherNavigation!.Name,
                    ClientCode = guarantee.EventProviderNavigation.Event.VoucherNavigation.Client.Code,
                    ClientId = guarantee.EventProviderNavigation.Event.VoucherNavigation.Client.Id,
                    EventStatusName = guarantee.EventProviderNavigation.Event.EventStatus_Name,
                    EventStatusId = guarantee.EventProviderNavigation.Event.EventStatusId
                })
                .FirstOrDefaultAsync();
        }
    }
}
