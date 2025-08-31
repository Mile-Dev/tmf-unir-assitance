using ProviderService.Domain.Dto.Provider;
using ProviderService.Domain.Dto.Provider.Created;
using ProviderService.Domain.Dto.Provider.Query;

namespace ProviderService.Services.Interfaces
{
    public interface IProviderServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        Task<ProviderIdDto> CreateProviderAsync(ProviderCreateDto provider);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idprovider"></param>
        /// <returns></returns>
        Task<ProviderGetDto> GetProviderByIdAsync(string idprovider);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idprovider"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        Task<ProviderGetDto> UpdateProviderByIdAsync(string idprovider, ProviderCreateDto provider);

        /// <summary>
        /// Method for delete provider by id
        /// </summary>
        /// <param name="idprovider"></param>
        /// <returns></returns>
        Task<bool> DeleteProviderByIdAsync(string idprovider);

        /// <summary>
        /// Get provider by PK
        /// </summary>
        /// <param name="providerFilterCity">Id provider</param>
        /// <returns>Provider object</returns>
        Task<List<ProviderSearchGetDto>> ProviderSearchAsync(ProviderFilterCity providerFilterCity);
    }
}
