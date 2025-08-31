using AutoMapper;
using MasterRdsServices.Domain.Dto;
using MasterRdsServices.Infraestructura.DataAccess.Interface.EntitiesDao;

namespace MasterRdsServices.Services
{
    public class VoucherStatusServices(ILogger<VoucherStatusServices> logger, IMapper mapper, IVoucherStatusRepository voucherstatusDao) : IVoucherStatusServices
    {

        private readonly ILogger<VoucherStatusServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IVoucherStatusRepository _voucherstatusDao = voucherstatusDao;

        public List<VoucherStatusQueryDto> GetVoucherStatus()
        {
            try
            {
                var listvoucherstatus = _voucherstatusDao.GetAll();
                var getlistvoucherstatus = _mapper.Map<List<VoucherStatusQueryDto>>(listvoucherstatus);
                return getlistvoucherstatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while geting the voucherstatus: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<VoucherStatusQueryDto> GetVoucherStatusById(int id)
        {
            try
            {
                var voucherstatus = await _voucherstatusDao.GetByIdAsync(id);
                var voucherstatusrecord = _mapper.Map<VoucherStatusQueryDto>(voucherstatus);
                return voucherstatusrecord;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the VoucherStatusById: {Message}", ex.Message);
                throw;
            }
        }

    }
}
