using AutoMapper;
using ProviderService.Common.Constans;
using ProviderService.Domain.Dto.Provider.Query;
using ProviderService.Domain.Dto.ProviderLocation;
using ProviderService.Domain.Dto.ProviderLocation.Created;
using ProviderService.Domain.Entities;
using ProviderService.Domain.Interfaces;
using ProviderService.Services.Interfaces;
using SharedServices.Objects;
using System.Globalization;

namespace ProviderService.Services
{
    public class ProviderLocationServices(IProviderLocationRepository repository, IMapper mapper, ILogger<ProviderLocationServices> logger) : IProviderLocationServices
    {
        private readonly IProviderLocationRepository _repository = repository;
        private readonly ILogger<ProviderLocationServices> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<ProviderLocationIdDto> CreateProviderLocationAsync(string id, ProviderLocationCreatedDto provider)
        {
            var idProvider = GenerateId(Constans.ProviderStartWith, id);
            var providerRetrieve = await _repository.GetProviderMetaDataByIdAsync(idProvider);
            if (providerRetrieve is null)
            {
                return new ProviderLocationIdDto() { IdLocation = "", IdProvider = "" };
            }
            var uuidGenerated = Guid.NewGuid().ToString();

            string geohash = GenerateGeohash(provider);

            ProviderLocation providerLocation = CreatedProviderLocation(id, provider, idProvider, providerRetrieve, uuidGenerated, GenerateId(Constans.LocationStartWith, uuidGenerated));
            providerLocation.Geohash = geohash;
            providerLocation.GeohashPk = string.Concat("geohashprefix#", geohash.AsSpan(0, 2));
            await _repository.CreateProviderLocationAsync(providerLocation);

            return _mapper.Map<ProviderLocationIdDto>(providerLocation);
        }

        private static string GenerateGeohash(ProviderLocationCreatedDto provider)
        {
            var latitude = Convert.ToDouble(provider.Latitude, CultureInfo.InvariantCulture);
            var longitude = Convert.ToDouble(provider.Longitude, CultureInfo.InvariantCulture);
            var geohash = GeohashHelper.Encode(latitude, longitude, 6);
            return geohash;
        }

        private static ProviderLocation CreatedProviderLocation(string id, ProviderLocationCreatedDto provider, string idProvider, ProviderLocation providerRetrieve, string uuidGenerated, string idassignedLocation)
        {
            return new()
            {
                PartitionKey = idProvider,
                ClasificationKey = idassignedLocation,
                IdProvider = id,
                IdLocation = uuidGenerated,
                Address = provider.Address,
                IdCity = provider.IdCity,
                City = provider.City,
                IdCountry = provider.IdCountry,
                Country = provider.Country,
                Latitude = provider.Latitude,
                Longitude = provider.Longitude,
                NameProvider = providerRetrieve.NameProvider,
                Score = providerRetrieve.Score,
                TypeProvider = providerRetrieve.TypeProvider,
                Details = provider.Details,
                CreatedAt = DateTime.UtcNow.ToString("o"),
                UpdatedAt = DateTime.UtcNow.ToString("o")
            };
        }

        private static string GenerateId(string suffix, string id) => suffix + id;

        public async Task<bool> DeleteProviderLocationByIdAsync(string idprovider, string idlocation)
        {
            var providerLocationRetrieve = await _repository.GetProviderLocationByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                             GenerateId(Constans.LocationStartWith, idlocation));
            if (providerLocationRetrieve is null) { return false; }
            var result = await _repository.DeleteProviderLocationAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                        GenerateId(Constans.LocationStartWith, idlocation));
            return result;
        }

        public async Task<ProviderLocationGetDto?> GetProviderLocationByIdAsync(string idprovider, string idlocation)
        {
            var providerLocationRetrieve = await _repository.GetProviderLocationByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                                          GenerateId(Constans.LocationStartWith, idlocation));
            if (providerLocationRetrieve is null) { return null; }
            var providerLocation = _mapper.Map<ProviderLocationGetDto>(providerLocationRetrieve);
            return providerLocation;
        }

        public async Task<ProviderLocationGetDto?> UpdateProviderLocationByIdAsync(string idprovider, string idlocation, ProviderLocationCreatedDto provider)
        {
            var providerLocationtRetrieve = await _repository.GetProviderLocationByIdAsync(GenerateId(Constans.ProviderStartWith, idprovider),
                                                                                           GenerateId(Constans.LocationStartWith, idlocation));
            if (providerLocationtRetrieve is null) { return null; }

            string geohash = GenerateGeohash(provider);

            providerLocationtRetrieve.Latitude = provider.Latitude;
            providerLocationtRetrieve.Longitude = provider.Longitude;
            providerLocationtRetrieve.IdCountry = provider.IdCountry;
            providerLocationtRetrieve.Country = provider.Country;
            providerLocationtRetrieve.IdCity = provider.IdCity;
            providerLocationtRetrieve.City = provider.City;
            providerLocationtRetrieve.Geohash = geohash;
            providerLocationtRetrieve.GeohashPk = string.Concat("geohashprefix#", geohash.AsSpan(0, 2));
            providerLocationtRetrieve.Address = provider.Address;
            providerLocationtRetrieve.Details = provider.Details;
            providerLocationtRetrieve.UpdatedAt = DateTime.Now.ToString("o");

            var result = await _repository.UpdateProviderLocationAsync(providerLocationtRetrieve);
            if (!result)
            {
                throw new Exception("fail to persist of Data");
            }
            return _mapper.Map<ProviderLocationGetDto>(providerLocationtRetrieve);

        }

        public async Task<PaginatedDataQueryDto> GetLocationByIdProviderAsync(string idprovider, Filters parameterGetList)
        {
            var PageNumber = parameterGetList.ParameterGetList.PageNumber >= 1 ? parameterGetList.ParameterGetList.PageNumber : 1;
            var PageSize = parameterGetList.ParameterGetList.PageSize >= 1 ? parameterGetList.ParameterGetList.PageSize : 10;

            var providerLocationtAll = await _repository.GetAllLocationsByProviderAsync(GenerateId(Constans.ProviderStartWith, idprovider));

            var paginatedResults = providerLocationtAll
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return new PaginatedDataQueryDto(paginatedResults, providerLocationtAll.Count());
        }
    }
}
