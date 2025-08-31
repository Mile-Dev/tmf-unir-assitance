using AutoMapper;
using ProviderService.Common.Constans;
using ProviderService.Domain.Dto.ProviderAgreement.Created;
using ProviderService.Domain.Dto.ProviderAgreement.Query;
using ProviderService.Domain.Entities;
using ProviderService.Domain.Interfaces;
using ProviderService.Services.Interfaces;

namespace ProviderService.Services
{
    public class ProviderAgreementServices(IProviderAgreementRepository repository, IMapper mapper, ILogger<ProviderAgreementServices> logger) : IProviderAgreementServices
    {
        private readonly IProviderAgreementRepository _repository = repository;
        private readonly ILogger<ProviderAgreementServices> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<ProviderAgreementIdDto> CreateProviderAgreementAsync(string id, ProviderAgreementCreatedDto provider)
        {
            var idProvider = GenerateId(Constans.ProviderStartWith, id);
            var providerRetrieve = await _repository.GetProviderMetaDataByIdAsync(idProvider);
            if (providerRetrieve is null)
            {
                return new ProviderAgreementIdDto() { IdAgreement = "", IdProvider = "" };
            }
            var uuidGenerated = Guid.NewGuid().ToString();
            ProviderAgreement providerAgreement = CreateProviderAgreement(idProvider, GenerateId(Constans.AgreementStartWith, uuidGenerated), uuidGenerated, id, provider);
            await _repository.CreateProviderAgreementAsync(providerAgreement);
            return _mapper.Map<ProviderAgreementIdDto>(providerAgreement);
        }

        private static string GenerateId(string suffix, string id) => suffix + id;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idProvider"></param>
        /// <param name="agreementId"></param>
        /// <param name="uuidGenerated"></param>
        /// <param name="id"></param>
        /// <param name="providerDto"></param>
        /// <returns></returns>
        private static ProviderAgreement CreateProviderAgreement(string idProvider, string agreementId, string uuidGenerated, string id, ProviderAgreementCreatedDto providerDto)
        {
            return new ProviderAgreement
            {
                PartitionKey = idProvider,
                ClasificationKey = agreementId,
                IdProvider = id,
                IdAgreement = uuidGenerated,
                EndValidity = providerDto.EndValidity,
                StartValidity = providerDto.StartValidity,
                UrlAttach = providerDto.UrlAttach,
                CreatedAt = DateTime.Now.ToString("o"),
                UpdatedAt = DateTime.Now.ToString("o")
            };
        }

        public async Task<bool> DeleteProviderAgreementByIdAsync(string idprovider, string idagreement)
        {
            var providerAgreementRetrieve = await _repository.GetProviderAgreementByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                              GenerateId(Constans.AgreementStartWith, idagreement));
            if (providerAgreementRetrieve is null) { return false; }
            var result = await _repository.DeleteProviderAgreementAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                        GenerateId(Constans.AgreementStartWith, idagreement));
            return result;
        }

        public async Task<ProviderAgreementGetDto?> GetProviderAgreementByIdAsync(string idprovider, string idagreement)
        {
            var providerAgreementRetrieve = await _repository.GetProviderAgreementByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                                          GenerateId(Constans.AgreementStartWith, idagreement));
            if (providerAgreementRetrieve is null) { return null; }
            var providerAgreement = _mapper.Map<ProviderAgreementGetDto>(providerAgreementRetrieve);
            return providerAgreement;
        }

        public async Task<ProviderAgreementGetDto?> UpdateProviderAgreementByIdAsync(string idprovider, string idagreement, ProviderAgreementCreatedDto provider)
        {
            var providerAgreementRetrieve = await _repository.GetProviderAgreementByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                                          GenerateId(Constans.AgreementStartWith, idagreement));
            if (providerAgreementRetrieve is null) { return null; }

            providerAgreementRetrieve.StartValidity = provider.StartValidity;
            providerAgreementRetrieve.EndValidity = provider.EndValidity;
            providerAgreementRetrieve.UrlAttach = provider.UrlAttach;
            providerAgreementRetrieve.UpdatedAt = DateTime.Now.ToString("o");

            var result = await _repository.UpdateProviderAgreementAsync(providerAgreementRetrieve);
            if (!result)
            {
                throw new Exception("fail to persist of Data");
            }
            return _mapper.Map<ProviderAgreementGetDto>(providerAgreementRetrieve);
        }
    }
}
