using AutoMapper;
using ProviderService.Common.Constans;
using ProviderService.Domain.Dto.ProviderContact.Created;
using ProviderService.Domain.Dto.ProviderContact.Query;
using ProviderService.Domain.Entities;
using ProviderService.Domain.Interfaces;
using ProviderService.Services.Interfaces;

namespace ProviderService.Services
{
    public class ProviderContactServices(IProviderContactRepository repository, ILogger<ProviderContactServices> logger, IMapper mapper) : IProviderContactServices
    {

        private readonly IProviderContactRepository _repository = repository;
        private readonly ILogger<ProviderContactServices> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<ProviderContactGetDto> CreateProviderContactAsync(string id, ProviderContactCreatedDto provider)
        {

            var idProvider = GenerateId(Constans.ProviderStartWith, id);
            var providerRetrieve = await _repository.GetProviderMetaDataByIdAsync(idProvider);
            if (providerRetrieve is null)
            {
                return new ProviderContactGetDto() { };
            }
            var uuidGenerated = Guid.NewGuid().ToString();
            ProviderContact providerContact = CreateObjectProviderContactAsync(idProvider, GenerateId(Constans.ContactStartWith, uuidGenerated), uuidGenerated, id, provider);
            await _repository.CreateProviderContactAsync(providerContact);

            var result = _mapper.Map<ProviderContactGetDto>(providerContact);
            return result;

        }

        private static string GenerateId(string suffix, string id) => suffix + id;

        public async Task<List<ProviderContactGetDto>> GetProviderContactAllByIdAsync(string idprovider)
        {
            var providerContactAll = await _repository.GetAllContactsByPkProviderAsync(GenerateId(Constans.ProviderStartWith, idprovider));
            var providerContacts = _mapper.Map<List<ProviderContactGetDto>>(providerContactAll);
            return providerContacts;
        }

        private static ProviderContact CreateObjectProviderContactAsync(string idProvider, string contactId, string uuidGenerated, string id, ProviderContactCreatedDto providerDto) => new ProviderContact
        {
            PartitionKey = idProvider,
            ClasificationKey = contactId,
            IdProvider = id,
            IdContact = uuidGenerated,
            ListData = providerDto.ListData.Select(item => new ListData() { Key = item.Key, Value = item.Value }).ToList(),
            Details = providerDto.Details,
            CreatedAt = DateTime.Now.ToString("o"),
            UpdatedAt = DateTime.Now.ToString("o")
        };

        public async Task<bool> DeleteProviderContactByIdAsync(string idprovider, string idcontact)
        {
            var providerAgreementRetrieve = await _repository.GetProviderContactByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                               GenerateId(Constans.ContactStartWith, idcontact));
            if (providerAgreementRetrieve is null) { return false; }
            var result = await _repository.DeleteProviderContactAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                        GenerateId(Constans.ContactStartWith, idcontact));
            return result;
        }

        public async Task<ProviderContactGetDto?> GetProviderContactByIdAsync(string idprovider, string idcontact)
        {
            var providerContactRetrieve = await _repository.GetProviderContactByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                                          GenerateId(Constans.ContactStartWith, idcontact));
            if (providerContactRetrieve is null) { return null; }
            var providerContact = _mapper.Map<ProviderContactGetDto>(providerContactRetrieve);
            return providerContact;
        }

        public async Task<ProviderContactGetDto?> UpdateProviderContactByIdAsync(string idprovider, string idcontact, ProviderContactCreatedDto provider)
        {
            var providerContactRetrieve = await _repository.GetProviderContactByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                                          GenerateId(Constans.ContactStartWith, idcontact));
            if (providerContactRetrieve is null) { return null; }

            providerContactRetrieve.ListData = provider.ListData.Select(item => new ListData() { Key = item.Key, Value = item.Value }).ToList();
            providerContactRetrieve.Details = provider.Details;
            providerContactRetrieve.UpdatedAt = DateTime.Now.ToString("o");

            var result = await _repository.UpdateProviderContactAsync(providerContactRetrieve);
            if (!result)
            {
                throw new Exception("fail to persist of Data");
            }
            return _mapper.Map<ProviderContactGetDto>(providerContactRetrieve);
        }

    }
}
