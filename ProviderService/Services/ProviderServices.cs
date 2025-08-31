using AutoMapper;
using ProviderService.Common.Constans;
using ProviderService.Domain.Dto;
using ProviderService.Domain.Dto.Provider;
using ProviderService.Domain.Dto.Provider.Created;
using ProviderService.Domain.Dto.Provider.Query;
using ProviderService.Domain.Entities;
using ProviderService.Domain.Interfaces;
using ProviderService.Services.Interfaces;
using System.Globalization;

namespace ProviderService.Services
{
    public class ProviderServices(IProviderRepository repository, IMapper mapper, ILogger<ProviderServices> logger) : IProviderServices
    {
        private readonly IProviderRepository _repository = repository;
        private readonly ILogger<ProviderServices> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<ProviderIdDto> CreateProviderAsync(ProviderCreateDto provider)
        {
            try
            {
                _logger.LogInformation("Entry method the service CreateProviderAsync");
                Console.WriteLine("TEST: Entry method the service CreateProviderAsync");

                var providerObject = await _repository.GetByNameAsync(provider.NameProvider.ToUpper().Trim(), Constans.NameFieldNameProvider);

                if (providerObject != null)
                {
                    throw new ArgumentException("Provider already exists");
                }
                var providerData = CreateObjectProvider(provider);
                await _repository.CreateAsync(providerData);
                var providerResult = _mapper.Map<ProviderIdDto>(providerData);
                _logger.LogInformation("Succes method the service CreateProviderAsync");
                Console.WriteLine("TEST: Succes method the service CreateProviderAsync");

                return providerResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine("TEST: error created provider");
                _logger.LogError(ex, "error created provider");
                throw;
            }
        }

        public Provider CreateObjectProvider(ProviderCreateDto provider)
        {
            _logger.LogInformation("Entry method the service CreateObjectProvider");
            var uuidGenerated = Guid.NewGuid().ToString();
            var idassignedProvider = Constans.ProviderStartWith + uuidGenerated;
            _logger.LogInformation("Entry method the service CreateObjectProvider", idassignedProvider);
            return new Provider
            {
                PartitionKey = idassignedProvider,
                ClasificationKey = idassignedProvider,
                IdProvider = uuidGenerated,
                NameProvider = provider.NameProvider.ToUpper().Trim(),
                Score = provider.Score,
                TypeProvider = provider.TypeProvider,
                Details = provider.Details,
                Nit = provider.Nit,
                IdFiscal = provider.IdFiscal
            };
        }

        public async Task<bool> DeleteProviderByIdAsync(string idprovider)
        {

            var idProviderMetadata = Constans.ProviderStartWith + idprovider;
            var provider = await _repository.GetByIdAllMetadataAsync(idProviderMetadata);
            if (provider.Count > 1)
            {
                throw new ArgumentException("Provider contains additional data, delete related information");
            }

            return await _repository.DeleteAsync(idProviderMetadata);
        }

        public async Task<ProviderGetDto> GetProviderByIdAsync(string idprovider)
        {
            try
            {
                _logger.LogInformation("Start method the service GetProviderByIdAsync");

                var idProviderMetadata = Constans.ProviderStartWith + idprovider;
                var provider = await _repository.GetByIdAllMetadataAsync(idProviderMetadata);
                var providerMetadata = provider.FirstOrDefault(item => item.ClasificationKey.Equals(idProviderMetadata));
                if (providerMetadata is null)
                {
                    return new ProviderGetDto { Id = "", NameProvider = "" };
                }
                ProviderGetDto providerGet = MapProviderData(provider, providerMetadata);
                _logger.LogInformation("Succes method the service GetProviderByIdAsync");
                return providerGet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving provider with id {IdProvider}.", idprovider);
                throw;
            }
        }

        private ProviderGetDto MapProviderData(IList<Provider> provider, Provider? providerMetadata)
        {
            var providerLocation = provider.Where(item => item.ClasificationKey.StartsWith(Constans.LocationStartWith)).ToList();
            var providerAgreement = provider.Where(item => item.ClasificationKey.StartsWith(Constans.AgreementStartWith)).ToList();
            var providerContact = provider.Where(item => item.ClasificationKey.StartsWith(Constans.ContactStartWith)).ToList();
            var providerPayment = provider.Where(item => item.ClasificationKey.StartsWith(Constans.PaymentMethodStartWith)).ToList();


            var providerGet = new ProviderGetDto
            {
                Id = providerMetadata.IdProvider,
                NameProvider = providerMetadata.NameProvider,
                Score = providerMetadata.Score,
                TypeProvider = providerMetadata.TypeProvider,
                Nit = providerMetadata.Nit,
                IdFiscal = providerMetadata.IdFiscal,
                Details = providerMetadata.Details,
                Locations = _mapper.Map<List<ProviderLocationGetDto>>(providerLocation),
                Agreements = _mapper.Map<List<ProviderAgreementDto>>(providerAgreement),
                Contacts = _mapper.Map<List<ProviderContactDto>>(providerContact),
                PaymentMethod = _mapper.Map<List<ProviderPaymentMethodDto>>(providerPayment)
            };
            return providerGet;
        }

        public async Task<ProviderGetDto> UpdateProviderByIdAsync(string idprovider, ProviderCreateDto provider)
        {
            _logger.LogInformation("Entry method the service UpdateProviderByIdAsync");
            var idProviderMetadata = Constans.ProviderStartWith + idprovider;
            var providerRetrieve = await _repository.GetByIdAsync(idProviderMetadata) ?? throw new ArgumentException("The provider does not exist");
            providerRetrieve.NameProvider = provider.NameProvider;
            providerRetrieve.Score = provider.Score;
            providerRetrieve.TypeProvider = provider.TypeProvider;
            providerRetrieve.Nit = provider.Nit;
            providerRetrieve.IdFiscal = provider.IdFiscal;
            providerRetrieve.Details = provider.Details;

            await _repository.UpdateAsync(providerRetrieve);

            var providerLocation = await _repository.GetByIdAllMetadataAsync(idProviderMetadata);
            var locationProvider = providerLocation.Where(item => item.ClasificationKey.StartsWith(Constans.LocationStartWith)).ToList();
            await UpdateRelationalData(provider, locationProvider);
            var providerResult = _mapper.Map<ProviderGetDto>(providerRetrieve);
            _logger.LogInformation("Succes method the service UpdateProviderByIdAsync");
            return providerResult;
        }

        private async Task UpdateRelationalData(ProviderCreateDto provider, List<Provider> locationProvider)
        {
            var locationProviders = new List<Provider>();
            if (locationProvider.Count > 0)
            {
                foreach (var location in locationProvider)
                {
                    location.NameProvider = provider.NameProvider;
                    location.TypeProvider = provider.TypeProvider;
                    location.UpdatedAt = DateTime.UtcNow.ToString("o");

                    locationProviders.Add(location);
                }
                await _repository.BatchWriteUpdateAsync(locationProviders);
            }
        }

        public async Task<List<ProviderSearchGetDto>> ProviderSearchAsync(ProviderFilterCity providerFilterCity)
        {
            try
            {
                _logger.LogInformation("Start method the service SearchAsync");

                if (string.IsNullOrWhiteSpace(providerFilterCity.IdCity)  && string.IsNullOrWhiteSpace(providerFilterCity.IdCountry) && string.IsNullOrWhiteSpace(providerFilterCity.Type))
                {
                    throw new ArgumentNullException(nameof(providerFilterCity), "Country or City is required");
                }

                string? geohash = null;
                if (!(string.IsNullOrWhiteSpace(providerFilterCity.Latitude) && string.IsNullOrWhiteSpace(providerFilterCity.Longitude)))
                {
                    geohash = GeohashHelper.Encode(Convert.ToDouble(providerFilterCity.Latitude, CultureInfo.InvariantCulture), Convert.ToDouble(providerFilterCity.Longitude, CultureInfo.InvariantCulture), 6);
                }

                var requestQuery = new RequestQueryDto
                {
                    IdCountry = providerFilterCity.IdCountry,
                    IdCity = providerFilterCity.IdCity,
                    Name = providerFilterCity.Name,
                    Type = providerFilterCity.Type,
                    Latitude = providerFilterCity.Latitude,
                    Longitude = providerFilterCity.Longitude,
                    RadioKMS = providerFilterCity.RadioKMS ?? 10,
                    Geohash = geohash,
                    NameExists = !string.IsNullOrEmpty(providerFilterCity.Name),
                    TypeExists = !string.IsNullOrEmpty(providerFilterCity.Type),
                    Limit = providerFilterCity.Limit ?? 10
                };
                var provider = await _repository.ProviderSearchAsync(requestQuery);
                var result = _mapper.Map<List<ProviderSearchGetDto>>(provider);
                _logger.LogInformation("Succes method the service SearchAsync");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving provider with city {}.", providerFilterCity.IdCity);
                throw;
            }
        }
    }
}
