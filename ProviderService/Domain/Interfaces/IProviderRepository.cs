using Amazon.DynamoDBv2.DocumentModel;
using ProviderService.Domain.Dto.Provider;
using ProviderService.Domain.Entities;

namespace ProviderService.Domain.Interfaces;

/// <summary>
/// Sample DynamoDB Table provider CRUD
/// </summary>
public interface IProviderRepository
{
    /// <summary>
    /// Include new provider to the DynamoDB Table
    /// </summary>
    /// <param name="provider">provider to include</param>
    /// <returns>success/failure</returns>
    Task<bool> CreateAsync(Provider provider);

    /// <summary>
    /// Remove existing provider from DynamoDB Table
    /// </summary>
    /// <param name="provider">provider to remove</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(string idprovider);

    /// <summary>
    /// List provider from DynamoDb Table with items limit (default=10)
    /// </summary>
    /// <param name="limit">limit (default=10)</param>
    /// <returns>Collection of providers</returns>
    Task<IList<Provider>> GetProvidersAsync(int limit = 10);

    /// <summary>
    /// Get provider by name
    /// </summary>
    /// <param name="Name">name of provider</param>
    /// <returns>Provider object</returns>
    Task<Provider?> GetByNameAsync(string fieldValue, string fieldName);

    /// <summary>
    /// Get provider by PK
    /// </summary>
    /// <param name="Id">Id provider</param>
    /// <returns>Provider object</returns>
    Task<Provider> GetByIdAsync(string Id);

    /// <summary>
    /// Get provider by PK
    /// </summary>
    /// <param name="Id">Id provider</param>
    /// <returns>Provider object</returns>
    Task<IList<Provider>> GetByIdAllMetadataAsync(string Id);

    /// <summary>
    /// Update provider content
    /// </summary>
    /// <param name="provider">provider to be updated</param>
    /// <returns>Provider object</returns>
    Task<bool> UpdateAsync(Provider provider);

    /// <summary>
    /// Get provider by PK
    /// </summary>
    /// <param name="requestQuery">Id provider</param>
    /// <returns>Provider object</returns>
    Task<List<Provider>> ProviderSearchAsync(RequestQueryDto requestQuery);

    /// <summary>
    /// Update list of providers
    /// </summary>
    /// <param name="providersToUpdate">List of providers for update</param>
    /// <returns>Boolean true</returns>
    Task<bool> BatchWriteUpdateAsync(List<Provider> providersToUpdate);
}