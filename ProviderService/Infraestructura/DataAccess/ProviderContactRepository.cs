using Amazon.DynamoDBv2.DataModel;
using ProviderService.Common.Constans;
using ProviderService.Domain.Entities;
using ProviderService.Domain.Interfaces;

namespace ProviderService.Infraestructura.DataAccess
{
    public class ProviderContactRepository(IDynamoDBContext context, ILogger<ProviderContactRepository> logger) : IProviderContactRepository
    {
        private readonly IDynamoDBContext _context = context;

        private readonly ILogger<ProviderContactRepository> _logger = logger;

        public async Task<ProviderContact> CreateProviderContactAsync(ProviderContact ProviderContact)
        {
            try
            {
                await _context.SaveAsync(ProviderContact);
                _logger.LogInformation("Provider Contact {} is added", ProviderContact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method CreateProviderContactAsync");
                return new ProviderContact() { ClasificationKey = string.Empty, PartitionKey = string.Empty };
            }

            return ProviderContact;
        }

        public async Task<bool> DeleteProviderContactAsync(string idPk, string idSk)
        {
            try
            {
                await _context.DeleteAsync<ProviderContact>(idPk, idSk);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method DeleteProviderContactAsync");
                return false;
            }
        }

        public async Task<IEnumerable<ProviderContact>> GetAllContactsByPkProviderAsync(string idPk)
        {
            return await _context.QueryAsync<ProviderContact>(idPk, Amazon.DynamoDBv2.DocumentModel.QueryOperator.BeginsWith, [Constans.ContactStartWith]).GetRemainingAsync();
        }

        public async Task<ProviderContact> GetProviderContactByIdAsync(string idPk, string idSk)
        {
            _logger.LogInformation("Entry method the repository GetProviderContactByIdAsync");
            return await _context.LoadAsync<ProviderContact>(idPk, idSk);
        }

        public async Task<ProviderContact> GetProviderMetaDataByIdAsync(string Id)
        {
            _logger.LogInformation("Entry method the repository GetProviderMetaDataByIdAsync");
            return await _context.LoadAsync<ProviderContact>(Id, Id);
        }

        public async Task<bool> UpdateProviderContactAsync(ProviderContact ProviderContact)
        {
            try
            {
                await _context.SaveAsync(ProviderContact);
                _logger.LogInformation("Provider Contact {} is update sucessfull", ProviderContact);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method UpdateProviderContactAsync");
                return false;
            }
        }
    }
}
