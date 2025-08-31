using Amazon.DynamoDBv2.DataModel;

namespace MasterRdsServices.Domain.Entities.Dynamodb
{
    [DynamoDBTable("CountriesAndCities")]
    public class City
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("PK")]
        public required string PartitionKey { get; init; }

        [DynamoDBRangeKey]
        [DynamoDBProperty("SK")]
        public required string ClasificationKey { get; init; }

        [DynamoDBProperty("AdminName")]
        public string AdminName { get; set; } = string.Empty;

        [DynamoDBProperty("Ciudad")]
        public string Ciudad { get; set; } = string.Empty;

        [DynamoDBProperty("Country")]
        public string Country { get; set; } = string.Empty;

        [DynamoDBProperty("IdCity")]
        public string IdCity { get; set; } = string.Empty;

        [DynamoDBProperty("Latitude")]
        public string Latitude { get; set; } = string.Empty;

        [DynamoDBProperty("Longitude")]
        public string Longitude { get; set; } = string.Empty;

        [DynamoDBProperty("Place")]
        public string Place { get; set; } = string.Empty;

        [DynamoDBProperty("Population")]
        public string Population { get; set; } = string.Empty;

        [DynamoDBProperty("Name")]
        public string Name { get; set; } = string.Empty;

        [DynamoDBProperty("PopulationType")]
        public string PopulationType { get; set; } = string.Empty;
    }
}
