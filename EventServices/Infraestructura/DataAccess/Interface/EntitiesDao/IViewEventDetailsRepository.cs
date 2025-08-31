using EventServices.Domain.Entities;

namespace EventServices.Infraestructura.DataAccess.Interface.EntitiesDao
{
    public interface IViewEventDetailsRepository : IRepository<ViewEventDetail>
    {

        Task<ViewEventDetail?> GetByCode(string value, int? clientCode = null);

        Task<List<ViewEventDetail>> GetByVoucher(string value, int? clientCode = null);

    }
}
