using MasterRdsServices.Domain.Dto.Location.Query;

namespace MasterRdsServices.Services
{
    public interface ICountriesAndCitiesServices
    {
        Task<List<CountryDto>> GetCountriesAsync();

        Task<List<CityDto>> GetCitiesByCountryAsync(string id);
    }
}
