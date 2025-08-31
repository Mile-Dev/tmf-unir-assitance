using ProviderService.Domain.Dto.Provider.Query;
using ProviderService.Domain.Dto.ProviderLocation;
using ProviderService.Domain.Dto.ProviderLocation.Created;
using SharedServices.Objects;

namespace ProviderService.Services.Interfaces
{
    public interface IProviderLocationServices
    {
        Task<ProviderLocationIdDto> CreateProviderLocationAsync(string id, ProviderLocationCreatedDto provider);

        Task<ProviderLocationGetDto?> GetProviderLocationByIdAsync(string idprovider, string idlocation);

        Task<ProviderLocationGetDto?> UpdateProviderLocationByIdAsync(string idprovider, string idlocation, ProviderLocationCreatedDto provider);

        Task<bool> DeleteProviderLocationByIdAsync(string idprovider, string idlocation);

        Task<PaginatedDataQueryDto> GetLocationByIdProviderAsync(string idprovider, Filters parameterGetList);

    }
}
