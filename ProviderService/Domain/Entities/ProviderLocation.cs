using Amazon.DynamoDBv2.DataModel;

namespace ProviderService.Domain.Entities
{
    [DynamoDBTable("ProviderData")]
    public class ProviderLocation
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("PK")]
        public required string PartitionKey { get; init; }

        [DynamoDBRangeKey]
        [DynamoDBProperty("SK")]
        public required string ClasificationKey { get; init; }

        [DynamoDBProperty("idProvider")]
        public string IdProvider { get; set; } = string.Empty;

        [DynamoDBProperty("idLocation")]
        public string IdLocation { get; set; } = string.Empty;

        [DynamoDBProperty("nameProvider")]
        public string NameProvider { get; set; } = string.Empty;

        [DynamoDBProperty("typeProvider")]
        public string TypeProvider { get; set; } = string.Empty;

        [DynamoDBProperty("idCountry")]
        public string IdCountry { get; set; } = string.Empty;

        [DynamoDBProperty("country")]
        public string Country { get; set; } = string.Empty;

        [DynamoDBProperty("idCity")]
        public string IdCity { get; set; } = string.Empty;

        [DynamoDBProperty("city")]
        public string City { get; set; } = string.Empty;

        [DynamoDBProperty("address")]
        public string Address { get; set; } = string.Empty;

        [DynamoDBProperty("longitude")]
        public string Longitude { get; set; } = string.Empty;

        [DynamoDBProperty("latitude")]
        public string Latitude { get; set; } = string.Empty;

        [DynamoDBProperty("details")]
        public string Details { get; set; } = string.Empty;

        [DynamoDBProperty("score")]
        public int? Score { get; set; }

        [DynamoDBProperty("geohash")]
        public string Geohash { get; set; } = string.Empty;

        [DynamoDBProperty("geohashPK")]
        public string GeohashPk { get; set; } = string.Empty;

        [DynamoDBProperty("createdAt")]
        public string CreatedAt { get; set; } = string.Empty;

        [DynamoDBProperty("updatedAt")]
        public string UpdatedAt { get; set; } = string.Empty;
    }
}
