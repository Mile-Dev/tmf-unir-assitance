using ProviderService.Domain.Dto.ProviderAgreement.Created;
using ProviderService.Domain.Dto.ProviderAgreement.Query;

namespace ProviderService.Services.Interfaces
{
    public interface IProviderAgreementServices
    {
        Task<ProviderAgreementIdDto> CreateProviderAgreementAsync(string id, ProviderAgreementCreatedDto provider);

        Task<ProviderAgreementGetDto?> GetProviderAgreementByIdAsync(string idprovider, string idagreement);

        Task<ProviderAgreementGetDto?> UpdateProviderAgreementByIdAsync(string idprovider, string idagreement, ProviderAgreementCreatedDto provider);

        Task<bool> DeleteProviderAgreementByIdAsync(string idprovider, string idagreement);
    }
}
