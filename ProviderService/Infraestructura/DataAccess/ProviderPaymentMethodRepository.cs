using Amazon.DynamoDBv2.DataModel;
using ProviderService.Common.Constans;
using ProviderService.Domain.Entities;
using ProviderService.Domain.Interfaces;

namespace ProviderService.Infraestructura.DataAccess
{
    public class ProviderPaymentMethodRepository(IDynamoDBContext context, ILogger<ProviderPaymentMethodRepository> logger) : IProviderPaymentMethodRepository
    {
        private readonly IDynamoDBContext _context = context;

        private readonly ILogger<ProviderPaymentMethodRepository> _logger = logger;

        public async Task<ProviderPaymentMethod> CreateProviderPaymentMethodAsync(ProviderPaymentMethod ProviderPaymentMethod)
        {
            try
            {
                await _context.SaveAsync(ProviderPaymentMethod);
                _logger.LogInformation("Provider Agreement {} is added", ProviderPaymentMethod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method CreateProviderAgreementAsync");
                return new ProviderPaymentMethod() { ClasificationKey = string.Empty, PartitionKey = string.Empty };
            }

            return ProviderPaymentMethod;
        }

        public async Task<bool> DeleteProviderPaymentMethodAsync(string idPk, string idSk)
        {
            try
            {
                await _context.DeleteAsync<ProviderPaymentMethod>(idPk, idSk);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method UpdateProviderAgreementAsync");
                return false;
            }
        }

        public async Task<IEnumerable<ProviderPaymentMethod>> GetAllProviderPaymentMethodsAsync(string idPk)
        {
            return await _context.QueryAsync<ProviderPaymentMethod>(idPk, Amazon.DynamoDBv2.DocumentModel.QueryOperator.BeginsWith, [Constans.PaymentMethodStartWith]).GetRemainingAsync();

        }

        public async Task<ProviderPaymentMethod> GetProviderMetaDataByIdAsync(string Id)
        {
            _logger.LogInformation("Entry method the repository GetProviderMetaDataByIdAsync");
            return await _context.LoadAsync<ProviderPaymentMethod>(Id, Id);
        }

        public async Task<ProviderPaymentMethod> GetProviderPaymentMethodByIdAsync(string idPk, string idSk)
        {
            _logger.LogInformation("Entry method the repository GetProviderPaymentMethodByIdAsync");
            return await _context.LoadAsync<ProviderPaymentMethod>(idPk, idSk);
        }

        public async Task<bool> UpdateProviderPaymentMethodAsync(ProviderPaymentMethod ProviderPaymentMethod)
        {
            try
            {
                await _context.SaveAsync(ProviderPaymentMethod);
                _logger.LogInformation("Provider ProviderPayment {} is update sucessfull", ProviderPaymentMethod);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table ProviderData in method UpdateProviderPaymentMethodAsync");
                return false;
            }
        }
    }
}
