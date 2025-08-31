using VoucherService.Domain.Dto;
using VoucherService.Infraestructura.EndpointSetw;

namespace VoucherService.Services
{
    public class CoverageServices(IInvokeIntegrationSetwCoverage invokeIntegrationSetwCoverage) : ICoverageServices
    {
        private readonly IInvokeIntegrationSetwCoverage _invokeIntegrationSetwCoverage = invokeIntegrationSetwCoverage;

        public async Task<List<ResponseCoverage>> GetEventAsync(string nameVoucher)
        {
            try
            {
                var coverageObject = await _invokeIntegrationSetwCoverage.GetSetwCoveragebyNumber(nameVoucher);
                return coverageObject;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
