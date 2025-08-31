using ProviderService.Domain.Dto.ProviderContact.Created;
using ProviderService.Domain.Dto.ProviderContact.Query;

namespace ProviderService.Services.Interfaces
{
    public interface IProviderContactServices
    {
        Task<ProviderContactGetDto> CreateProviderContactAsync(string id, ProviderContactCreatedDto provider);

        Task<ProviderContactGetDto?> GetProviderContactByIdAsync(string idprovider, string idcontact);

        Task<ProviderContactGetDto?> UpdateProviderContactByIdAsync(string idprovider, string idcontact, ProviderContactCreatedDto provider);

        Task<bool> DeleteProviderContactByIdAsync(string idprovider, string idcontact);

        Task<List<ProviderContactGetDto>> GetProviderContactAllByIdAsync(string idprovider);

    }
}
