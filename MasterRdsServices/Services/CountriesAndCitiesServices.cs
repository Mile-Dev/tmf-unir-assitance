using AutoMapper;
using MasterRdsServices.Domain.Dto.Location.Query;
using MasterRdsServices.Infraestructura.DataAccesDynamo.Interfaces;

namespace MasterRdsServices.Services
{
    public class CountriesAndCitiesServices(ICountriesAndCitiesRepository countriesAndCitiesRepository, ILogger<CountriesAndCitiesServices> logger, IMapper mapper) : ICountriesAndCitiesServices
    {
        private readonly ICountriesAndCitiesRepository _countriesAndCitiesRepository = countriesAndCitiesRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CountriesAndCitiesServices> _logger = logger;

        public async Task<List<CityDto>> GetCitiesByCountryAsync(string id)
        {
            try
            {
                _logger.LogInformation("Get cities by country");
                var recordsCity = await _countriesAndCitiesRepository.GetCityByCountryAsync(id);
                var cities = _mapper.Map<List<CityDto>>(recordsCity);
                return cities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cities by country");
                throw;
            }
        }

        public async Task<List<CountryDto>> GetCountriesAsync()
        {
            try
            {
                _logger.LogInformation("Get countries");
                var recordsCountries = await _countriesAndCitiesRepository.GetCountriesAsync();
                var listcountries = _mapper.Map<List<CountryDto>>(recordsCountries);
                return listcountries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting countries by country");
                throw;
            }
        }
    }
}
