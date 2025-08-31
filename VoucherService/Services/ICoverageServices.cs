using VoucherService.Domain.Dto;

namespace VoucherService.Services
{
    public interface ICoverageServices
    {
        Task<List<ResponseCoverage>> GetEventAsync(string nameVoucher);
    }
}
