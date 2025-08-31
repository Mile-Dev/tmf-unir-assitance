using ProviderService.Domain.Dto.ProviderPaymentMethod.Created;
using ProviderService.Domain.Dto.ProviderPaymentMethod.Query;

namespace ProviderService.Services.Interfaces
{
    public interface IProviderPaymentMethodServices
    {
        Task<ProviderPaymentMethodGetDto> CreateProviderPaymentMethodAsync(string id, ProviderPaymentMethodCreatedDto provider);

        Task<ProviderPaymentMethodGetDto?> GetProviderPaymentMethodByIdAsync(string idprovider, string idpaymentmethod);

        Task<ProviderPaymentMethodGetDto?> UpdateProviderPaymentMethodByIdAsync(string idprovider, string idpaymentmethod, ProviderPaymentMethodCreatedDto provider);

        Task<bool> DeleteProviderPaymentMethodByIdAsync(string idprovider, string idpaymentmethod);

    }
}
