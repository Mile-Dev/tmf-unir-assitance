using Amazon.DynamoDBv2.DataModel;
using MasterRdsServices.Common;
using MasterRdsServices.Domain.Entities.Dynamodb;
using MasterRdsServices.Infraestructura.DataAccesDynamo.Interfaces;

namespace MasterRdsServices.Infraestructura.DataAccesDynamo
{
    public class CountriesAndCitiesRepository(IDynamoDBContext context, ILogger<CountriesAndCitiesRepository> logger) : ICountriesAndCitiesRepository
    {
        private readonly IDynamoDBContext _context = context;

        private readonly ILogger<CountriesAndCitiesRepository> _logger = logger;

        public async Task<IList<City>> GetCityByCountryAsync(string Id)
        {
            try
            {
                var resultcitylist = await _context.QueryAsync<City>(Id).GetRemainingAsync();
                return resultcitylist;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to update provider from DynamoDb Table");
                throw;
            }
        }

        public async Task<IList<Country>> GetCountriesAsync()
        {
            try
            {
                var resultcountrylist = await _context.QueryAsync<Country>(Constans.PKCOUNTRY).GetRemainingAsync();
                return resultcountrylist;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to update provider from DynamoDb Table");
                throw;
            }
        }
    }
}
