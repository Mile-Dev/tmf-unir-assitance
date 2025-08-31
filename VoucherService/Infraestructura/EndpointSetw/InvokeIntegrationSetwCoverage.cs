using VoucherService.Common.Configuration;
using VoucherService.Common.HttpClient;
using VoucherService.Domain.Dto;

namespace VoucherService.Infraestructura.EndpointSetw
{
    public class InvokeIntegrationSetwCoverage : InvokeClientBase, IInvokeIntegrationSetwCoverage
    {
        private readonly IConfigurationApplication _configuration;
 
        public InvokeIntegrationSetwCoverage(IConfigurationApplication configuration) : base(new InvokeClientService(new Uri(configuration.UrlApiSetw)))
        {
            _configuration = configuration;
        
        }

        public async Task<List<ResponseCoverage>> GetSetwCoveragebyNumber(string numberVoucher)
        {
            var url = $"{_configuration.UrlApiSetw}{_configuration.PathSetwCoverages}?password={_configuration.PasswordSetwCoverages}&voucher_number={numberVoucher}";
            ApiInvoke(url);
            return await GetAsync<List<ResponseCoverage>>();
        }
    }
}
