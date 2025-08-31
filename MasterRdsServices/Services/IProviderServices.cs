using MasterRdsServices.Domain.Dto.Provider.Query;

namespace MasterRdsServices.Services
{
    public interface IProviderServices
    {
        Task<List<ProviderTypeGetDto>> GetRecords();
    }
}
