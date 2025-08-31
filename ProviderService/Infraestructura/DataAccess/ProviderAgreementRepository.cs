using Amazon.DynamoDBv2.DataModel;
using ProviderService.Common.Constans;
using ProviderService.Domain.Entities;
using ProviderService.Domain.Interfaces;

namespace ProviderService.Infraestructura.DataAccess
{
    public class ProviderAgreementRepository(IDynamoDBContext context, ILogger<ProviderAgreementRepository> logger) : IProviderAgreementRepository
    {
        private readonly IDynamoDBContext _context = context;

        private readonly ILogger<ProviderAgreementRepository> _logger = logger;

        public async Task<ProviderAgreement> CreateProviderAgreementAsync(ProviderAgreement ProviderAgreement)
        {
            try
            {
                await _context.SaveAsync(ProviderAgreement);
                _logger.LogInformation("Provider Agreement {} is added", ProviderAgreement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method CreateProviderAgreementAsync");
                return new ProviderAgreement() { ClasificationKey = string.Empty, PartitionKey = string.Empty };
            }

            return ProviderAgreement;
        }

        public async Task<bool> DeleteProviderAgreementAsync(string idPk, string idSk)
        {
            try
            {
                await _context.DeleteAsync<ProviderAgreement>(idPk, idSk);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method UpdateProviderAgreementAsync");
                return false;
            }
        }

        public async Task<IEnumerable<ProviderAgreement>> GetAllProviderAgreementsAsync(string idPk)
        {
            return await _context.QueryAsync<ProviderAgreement>(idPk, Amazon.DynamoDBv2.DocumentModel.QueryOperator.BeginsWith, [Constans.AgreementStartWith]).GetRemainingAsync();
        }

        public async Task<ProviderAgreement> GetProviderAgreementByIdAsync(string idPk, string idSk)
        {
            _logger.LogInformation("Entry method the repository GetProviderAgreementByIdAsync");
            return await _context.LoadAsync<ProviderAgreement>(idPk, idSk);
        }

        public async Task<ProviderAgreement> GetProviderMetaDataByIdAsync(string Id)
        {
            _logger.LogInformation("Entry method the repository GetProviderMetaDataByIdAsync");
            return await _context.LoadAsync<ProviderAgreement>(Id, Id);
        }

        public async Task<bool> UpdateProviderAgreementAsync(ProviderAgreement ProviderAgreement)
        {
            try
            {
                await _context.SaveAsync(ProviderAgreement);
                _logger.LogInformation("Provider Agreement {} is update sucessfull", ProviderAgreement);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method UpdateProviderAgreementAsync");
                return false;
            }
        }
    }
}
