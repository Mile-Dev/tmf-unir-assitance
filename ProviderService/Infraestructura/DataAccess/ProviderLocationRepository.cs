using Amazon.DynamoDBv2.DataModel;
using ProviderService.Common.Constans;
using ProviderService.Domain.Entities;
using ProviderService.Domain.Interfaces;

namespace ProviderService.Infraestructura.DataAccess
{
    public class ProviderLocationRepository(IDynamoDBContext context, ILogger<ProviderLocationRepository> logger) : IProviderLocationRepository
    {

        private readonly IDynamoDBContext _context = context;

        private readonly ILogger<ProviderLocationRepository> _logger = logger;

        public async Task<ProviderLocation> CreateProviderLocationAsync(ProviderLocation providerLocation)
        {
            try
            {
                await _context.SaveAsync(providerLocation);
                _logger.LogInformation("Provider Location {} is added", providerLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method CreateProviderLocationAsync ");
                return new ProviderLocation() { ClasificationKey = string.Empty, PartitionKey = string.Empty };
            }

            return providerLocation;
        }

        public async Task<bool> DeleteProviderLocationAsync(string idPk, string idSk)
        {
            try
            {
                await _context.DeleteAsync<ProviderLocation>(idPk, idSk);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method DeleteProviderLocationAsync");
                return false;
            }
        }

        public async Task<IEnumerable<ProviderLocation>> GetAllLocationsByProviderAsync(string idPk)
        {
            return await _context.QueryAsync<ProviderLocation>(idPk, Amazon.DynamoDBv2.DocumentModel.QueryOperator.BeginsWith, [Constans.LocationStartWith]).GetRemainingAsync();
        }

        public async Task<ProviderLocation> GetProviderLocationByIdAsync(string idPk, string idSk)
        {
            _logger.LogInformation("Entry method the repository GetProviderLocationByIdAsync");
            return await _context.LoadAsync<ProviderLocation>(idPk, idSk);
        }

        public async Task<bool> UpdateProviderLocationAsync(ProviderLocation providerLocation)
        {
            try
            {
                await _context.SaveAsync(providerLocation);
                _logger.LogInformation("Provider Agreement {} is update sucessfull", providerLocation);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method UpdateProviderLocationAsync");
                return false;
            }
        }

        public async Task<ProviderLocation> GetProviderMetaDataByIdAsync(string Id)
        {
            _logger.LogInformation("Entry method the repository GetProviderMetaDataByIdAsync");
            return await _context.LoadAsync<ProviderLocation>(Id, Id);
        }
    }
}
