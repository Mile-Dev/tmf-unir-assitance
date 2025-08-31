using EventServices.Domain.Entities;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    public interface IViewGuaranteesPaymentEventProviderRepository: IRepository<ViewGuaranteesPaymentEventProvider>
    {
        public Task<List<ViewGuaranteesPaymentEventProvider>> GetGuaranteesPaymentByIdEventProviderAsync(int id);
    }
}
