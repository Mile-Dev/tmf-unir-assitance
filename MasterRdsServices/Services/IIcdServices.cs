using MasterRdsServices.Domain.Dto.Icd.Query;

namespace MasterRdsServices.Services
{
    public interface IIcdServices
    {
        Task<List<IcdDto>> GetRecords(string? name);
    }
}
