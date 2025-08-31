using MasterRdsServices.Domain.Dto;

namespace MasterRdsServices.Services
{
    public interface IVoucherStatusServices
    {
        List<VoucherStatusQueryDto> GetVoucherStatus();

        Task<VoucherStatusQueryDto> GetVoucherStatusById(int id);


    }
}
