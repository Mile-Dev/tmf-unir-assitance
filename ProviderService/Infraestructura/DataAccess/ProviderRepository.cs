using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ProviderService.Domain.Dto.Provider;
using ProviderService.Domain.Entities;
using ProviderService.Domain.Interfaces;
using System.Globalization;
using System.Text;

namespace ProviderService.Infraestructura.DataAccess;

public class ProviderRepository(IDynamoDBContext context, ILogger<ProviderRepository> logger) : IProviderRepository
{
    private readonly IDynamoDBContext _context = context;

    private readonly ILogger<ProviderRepository> logger = logger;

    public async Task<bool> CreateAsync(Provider provider)
    {
        try
        {
            await _context.SaveAsync(provider);
            logger.LogInformation("Provider {} is added", provider);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "fail to persist to DynamoDb Table");
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteAsync(string idprovider)
    {
        bool result;
        try
        {
            await _context.DeleteAsync<Provider>(idprovider, idprovider);
            Provider deletedProvider = await _context.LoadAsync<Provider>(
                idprovider, idprovider, new DynamoDBOperationConfig
                {
                    ConsistentRead = true
                });

            result = deletedProvider == null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "fail to delete provider from DynamoDb Table");
            result = false;
        }

        if (result) logger.LogInformation("Provider {Id} is deleted", idprovider);

        return result;
    }

    public async Task<bool> UpdateAsync(Provider provider)
    {
        if (provider == null) return false;

        try
        {
            await _context.SaveAsync(provider);
            logger.LogInformation("Provider {Pk} is updated", provider);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "fail to update provider from DynamoDb Table");
            return false;
        }

        return true;
    }

    public async Task<bool> BatchWriteUpdateAsync(List<Provider> providersToUpdate)
    {
        if (providersToUpdate == null) return false;

        try
        {
            var prividerBatch = _context.CreateBatchWrite<Provider>();
            prividerBatch.AddPutItems(providersToUpdate);
            await prividerBatch.ExecuteAsync();

            logger.LogInformation("Location of provider {Pk} is updated", providersToUpdate);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "fail to update provider from DynamoDb Table");
            return false;
        }

        return true;
    }

    public async Task<Provider?> GetByPkSkAsync(string pk, string sk)
    {
        try
        {
            return await _context.LoadAsync<Provider>(pk, sk);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "fail to update provider from DynamoDb Table");
            return null;
        }
    }

    public async Task<IList<Provider>> GetProvidersAsync(int limit = 10)
    {
        var result = new List<Provider>();

        try
        {
            if (limit <= 0)
            {
                return result;
            }

            var filter = new ScanFilter();
            filter.AddCondition("idpartition", ScanOperator.IsNotNull);
            var scanConfig = new ScanOperationConfig()
            {
                Limit = limit,
                Filter = filter
            };
            var queryResult = _context.FromScanAsync<Provider>(scanConfig);

            do
            {
                result.AddRange(await queryResult.GetNextSetAsync());
            }
            while (!queryResult.IsDone && result.Count < limit);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "fail to list providers from DynamoDb Table");
            return new List<Provider>();
        }

        return result;
    }

    public async Task<Provider?> GetByNameAsync(string fieldValue, string fieldName)
    {
        var expressionAttributeValues = new Dictionary<string, DynamoDBEntry>
        {
            { ":v1", fieldValue },
            { ":v2", "provider#" }
        };

        var config = new QueryOperationConfig
        {
            KeyExpression = new Expression
            {
                ExpressionStatement = $"{fieldName} = :v1",
                ExpressionAttributeValues = expressionAttributeValues
            },
            FilterExpression = new Expression
            {
                ExpressionStatement = $"begins_with(SK, :v2)",
                ExpressionAttributeValues = expressionAttributeValues
            },
            IndexName = $"{fieldName}-index"
        };

        var resultProvider = await _context.FromQueryAsync<Provider>(config).GetRemainingAsync();

        return resultProvider.FirstOrDefault();
    }

    public async Task<Provider> GetByIdAsync(string Id)
    {
        logger.LogInformation("Entry method the repository GetByIdAsync");
        return await _context.LoadAsync<Provider>(Id, Id);
    }

    public async Task<IList<Provider>> GetByIdAllMetadataAsync(string Id)
    {
        return await _context.QueryAsync<Provider>(Id).GetRemainingAsync();
    }

    public async Task<List<Provider>> ProviderSearchAsync(RequestQueryDto request)
    {

        var requestQuery = CreateQueryRequest(request);
        var dataSearch = await _context.FromQueryAsync<Provider>(requestQuery).GetRemainingAsync();


        if (request.Geohash != null)
        {
            dataSearch = dataSearch.Where(doc =>
            {
                double lat = Convert.ToDouble(doc.Latitude, CultureInfo.InvariantCulture);
                double lon = Convert.ToDouble(doc.Longitude, CultureInfo.InvariantCulture);
                double distance = CalculateDistance(Convert.ToDouble(request.Latitude, CultureInfo.InvariantCulture), Convert.ToDouble(request.Longitude, CultureInfo.InvariantCulture), lat, lon);
                return distance <= request.RadioKMS;
            }).ToList();
        }
        return dataSearch;
    }

    private static QueryOperationConfig CreateQueryRequest(RequestQueryDto request)
    {
        var expressionBuilder = new StringBuilder();
        var filterexpressionBuilder = new StringBuilder();
        var expressionAttributeValues = new Dictionary<string, DynamoDBEntry>();
        var expressionAttributeNames = new Dictionary<string, string>();
        var expressionIndex = "";

        if (request.Geohash != null)
        {
            BuildGeohashExpression(request, expressionBuilder, expressionAttributeValues, ref expressionIndex);
        }
        else
        {
            BuildLocationExpression(request, expressionBuilder, expressionAttributeValues, ref expressionIndex);
            BuildFilterExpression(request, filterexpressionBuilder, expressionAttributeValues);
        }

        return new QueryOperationConfig()
        {
            IndexName = expressionIndex,
            KeyExpression = new Expression
            {
                ExpressionStatement = expressionBuilder.ToString(),
                ExpressionAttributeValues = expressionAttributeValues,
                ExpressionAttributeNames = expressionAttributeNames
            },
            FilterExpression = filterexpressionBuilder.Length > 0 ? new Expression
            {
                ExpressionStatement = filterexpressionBuilder.ToString(),
                ExpressionAttributeValues = expressionAttributeValues
            } : null,
            Limit = request.Limit,
            // PaginationToken = request.PaginationToken
        };
    }
    //Tomar decision de cual dejar 
    private static void BuildGeohashExpression(RequestQueryDto request, StringBuilder expressionBuilder, Dictionary<string, DynamoDBEntry> expressionAttributeValues, ref string expressionIndex)
    {
        expressionIndex = "geohashPK-index";
        expressionBuilder.Append("geohashPK = :v1");
        expressionAttributeValues.Add(":v1", "geohashprefix");
    }

    private static void BuildLocationExpression(RequestQueryDto request, StringBuilder expressionBuilder, Dictionary<string, DynamoDBEntry> expressionAttributeValues, ref string expressionIndex)
    {
        if (!string.IsNullOrEmpty(request.IdCity))
        {
            expressionIndex = "idCity-index";
            expressionBuilder.Append("idCity = :v1");
            expressionAttributeValues.Add(":v1", request.IdCity);
        }
        else if (!string.IsNullOrEmpty(request.IdCountry) && string.IsNullOrEmpty(request.IdCity))
        {
            expressionIndex = "idCountry-index";
            expressionBuilder.Append("idCountry = :v1");
            expressionAttributeValues.Add(":v1", request.IdCountry);
        }
    }

    private static void BuildFilterExpression(RequestQueryDto request, StringBuilder filterexpressionBuilder, Dictionary<string, DynamoDBEntry> expressionAttributeValues)
    {
        if (request.NameExists && !string.IsNullOrEmpty(request.Name))
        {
            if (filterexpressionBuilder.Length > 0) filterexpressionBuilder.Append(" AND ");
            filterexpressionBuilder.Append("contains(nameProvider, :v2)");
            expressionAttributeValues.Add(":v2", request.Name);
        }

        if (request.TypeExists && !string.IsNullOrEmpty(request.Type))
        {
            if (filterexpressionBuilder.Length > 0) filterexpressionBuilder.Append(" AND ");
            filterexpressionBuilder.Append("contains(typeProvider, :v3)");
            expressionAttributeValues.Add(":v3", request.Type);
        }
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double Radio = 6371;
        double dLat = ToRadians(lat2 - lat1);
        double dLon = ToRadians(lon2 - lon1);
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return Radio * c;
    }

    private double ToRadians(double degrees) => degrees * (Math.PI / 180);


}