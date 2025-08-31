using Amazon.DynamoDBv2.DataModel;

namespace EventFirstContactServices.Domain.Entities
{
    [DynamoDBTable("EventDraft")]
    public class EventLocation
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("PK")]
        public required string PartitionKey { get; set; } = string.Empty;

        [DynamoDBRangeKey]
        [DynamoDBProperty("SK")]
        public required string ClasificationKey { get; set; } = string.Empty;

        [DynamoDBProperty("createdAt")]
        public string? CreatedAt { get; set; } = null;

        [DynamoDBProperty("updatedAt")]
        public string? UpdatedAt { get; set; } = null;

        [DynamoDBProperty("countryEventLocation")]
        public string? CountryEventLocation { get; set; } = null;

        [DynamoDBProperty("cityEventLocation")]
        public string? CityEventLocation { get; set; } = null;

        [DynamoDBProperty("addressEventLocation")]
        public string? AddressEventLocation { get; set; } = null;

        [DynamoDBProperty("gpsEventLocation")]
        public string? GpsEventLocation { get; set; } = null;

        [DynamoDBProperty("informationEventLocation")]
        public string? InformationLocation { get; set; } = null;

    }

}
