using MasterRdsServices.Domain.Entities.Dynamodb;

namespace MasterRdsServices.Infraestructura.DataAccesDynamo.Interfaces
{
    public interface ICountriesAndCitiesRepository
    {
        /// <summary>
        /// List Country from DynamoDb Table
        /// </summary>
        /// <param name="limit">limit (default=10)</param>
        /// <returns>Collection of providers</returns>
        Task<IList<Country>> GetCountriesAsync();

        /// <summary>
        /// Get City by Country
        /// </summary>
        /// <param name="Id">Id of Country</param>
        /// <returns>Collection of City</returns>
        Task<IList<City>> GetCityByCountryAsync(string Id);
    }
}
