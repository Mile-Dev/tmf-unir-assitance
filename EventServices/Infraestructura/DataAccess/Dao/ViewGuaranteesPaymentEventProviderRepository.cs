using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para acceder a la vista de pagos de garantías por proveedor de evento.
    /// Hereda de Repository y proporciona métodos específicos para la entidad ViewGuaranteesPaymentEventProvider.
    /// </summary>
    public class ViewGuaranteesPaymentEventProviderRepository(MainContext context)
        : Repository<ViewGuaranteesPaymentEventProvider>(context), IViewGuaranteesPaymentEventProviderRepository
    {
        /// <summary>
        /// Obtiene de forma asíncrona la lista de pagos de garantías asociados a un proveedor de evento específico.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>Lista de objetos ViewGuaranteesPaymentEventProvider relacionados con el proveedor de evento.</returns>
        public async Task<List<ViewGuaranteesPaymentEventProvider>> GetGuaranteesPaymentByIdEventProviderAsync(int id)
        {
            var listGuaranteesPayment = await Entities.Where(x => x.EventProviderId == id).ToListAsync();
            return listGuaranteesPayment;
        }
    }
}
