using VoucherService.Domain.Dto;

namespace VoucherService.Infraestructura.EndpointSetw
{
    public interface IInvokeIntegrationSetwCoverage
    {
        Task<List<ResponseCoverage>> GetSetwCoveragebyNumber(string codFirstcontact);

    }
}
