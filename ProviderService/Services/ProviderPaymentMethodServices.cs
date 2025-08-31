using AutoMapper;
using ProviderService.Common.Constans;
using ProviderService.Domain.Dto.ProviderPaymentMethod.Created;
using ProviderService.Domain.Dto.ProviderPaymentMethod.Query;
using ProviderService.Domain.Entities;
using ProviderService.Domain.Interfaces;
using ProviderService.Services.Interfaces;

namespace ProviderService.Services
{
    public class ProviderPaymentMethodServices(IProviderPaymentMethodRepository repository, ILogger<ProviderPaymentMethodServices> logger, IMapper mapper) : IProviderPaymentMethodServices
    {

        private readonly IProviderPaymentMethodRepository _repository = repository;
        private readonly ILogger<ProviderPaymentMethodServices> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<ProviderPaymentMethodGetDto> CreateProviderPaymentMethodAsync(string id, ProviderPaymentMethodCreatedDto provider)
        {
            var idProvider = GenerateId(Constans.ProviderStartWith, id);
            var providerRetrieve = await _repository.GetProviderMetaDataByIdAsync(idProvider);
            if (providerRetrieve is null)
            {
                return new ProviderPaymentMethodGetDto() { };
            }
            var uuidGenerated = Guid.NewGuid().ToString();
            ProviderPaymentMethod providerPaymentMethod = CreateObjectProviderContactAsync(idProvider, GenerateId(Constans.PaymentMethodStartWith, uuidGenerated), uuidGenerated, id, provider);
            await _repository.CreateProviderPaymentMethodAsync(providerPaymentMethod);

            var result = _mapper.Map<ProviderPaymentMethodGetDto>(providerPaymentMethod);
            return result;
        }

        private static string GenerateId(string suffix, string id) => suffix + id;

        private static ProviderPaymentMethod CreateObjectProviderContactAsync(string idProvider, string paymentMethodId, string uuidGenerated, string id, ProviderPaymentMethodCreatedDto providerDto) => new ProviderPaymentMethod
        {
            PartitionKey = idProvider,
            ClasificationKey = paymentMethodId,
            IdProvider = id,
            IdPayment = uuidGenerated,
            TypePayment = providerDto.TypePayment,
            ListData = providerDto.ListData.Select(item => new ListData() { Key = item.Key, Value = item.Value }).ToList(),
            IsActivePayment = true,
            Details = providerDto.Details,
            CreatedAt = DateTime.Now.ToString("o"),
            UpdatedAt = DateTime.Now.ToString("o")
        };


        public async Task<bool> DeleteProviderPaymentMethodByIdAsync(string idprovider, string idpaymentmethod)
        {
            var providerPaymentMethodRetrieve = await _repository.GetProviderPaymentMethodByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                              GenerateId(Constans.PaymentMethodStartWith, idpaymentmethod));
            if (providerPaymentMethodRetrieve is null) { return false; }
            var result = await _repository.DeleteProviderPaymentMethodAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                        GenerateId(Constans.PaymentMethodStartWith, idpaymentmethod));
            return result;
        }

        public async Task<ProviderPaymentMethodGetDto?> GetProviderPaymentMethodByIdAsync(string idprovider, string idpaymentmethod)
        {
            var providerLocationRetrieve = await _repository.GetProviderPaymentMethodByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                                               GenerateId(Constans.PaymentMethodStartWith, idpaymentmethod));
            if (providerLocationRetrieve is null) { return null; }

            var providerLocation = _mapper.Map<ProviderPaymentMethodGetDto>(providerLocationRetrieve);
            return providerLocation;
        }

        public async Task<ProviderPaymentMethodGetDto?> UpdateProviderPaymentMethodByIdAsync(string idprovider, string idpaymentmethod, ProviderPaymentMethodCreatedDto provider)
        {
            var providerPaymentMethodRetrieve = await _repository.GetProviderPaymentMethodByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                                         GenerateId(Constans.PaymentMethodStartWith, idpaymentmethod));
            if (providerPaymentMethodRetrieve is null) { return null; }

            providerPaymentMethodRetrieve.TypePayment = provider.TypePayment;
            providerPaymentMethodRetrieve.ListData = provider.ListData.Select(item => new ListData() { Key = item.Key, Value = item.Value }).ToList();
            providerPaymentMethodRetrieve.Details = provider.Details;
            providerPaymentMethodRetrieve.UpdatedAt = DateTime.Now.ToString("o");

            var result = await _repository.UpdateProviderPaymentMethodAsync(providerPaymentMethodRetrieve);
            if (!result)
            {
                throw new Exception("fail to persist of Data");
            }
            return _mapper.Map<ProviderPaymentMethodGetDto>(providerPaymentMethodRetrieve);
        }
    }
}
