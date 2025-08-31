using Amazon.DynamoDBv2.DataModel;

namespace EventServices.EventFirstContact.Domain.Entities
{
    [DynamoDBTable("EventDraft")]
    public class EventProvider
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("PK")]
        public required string PartitionKey { get; set; } = string.Empty;

        [DynamoDBRangeKey]
        [DynamoDBProperty("SK")]
        public required string ClasificationKey { get; set; } = string.Empty;

        [DynamoDBProperty("idProvider")]
        public string IdProvider { get; set; } = string.Empty;

        [DynamoDBProperty("idLocation")]
        public string IdLocation { get; set; } = string.Empty;

        [DynamoDBProperty("createdAt")]
        public string? CreatedAt { get; set; } = null;

        [DynamoDBProperty("updatedAt")]
        public string? UpdatedAt { get; set; } = null;

        [DynamoDBProperty("countryEventProvider")]
        public string? CountryEventProvider { get; set; } = null;

        [DynamoDBProperty("cityEventProvider")]
        public string? CityEventProvider { get; set; } = null;

        [DynamoDBProperty("nearOfEventProvider")]
        public string? NearOfEventProvider { get; set; } = null;

        [DynamoDBProperty("addressEventProvider")]
        public string? AddressEventProvider { get; set; } = null;

        [DynamoDBProperty("informationEventProvider")]
        public string? InformationEventProvider { get; set; } = null;

        [DynamoDBProperty("gpsEventProvider")]
        public string? GpsEventProvider { get; set; } = null;

        [DynamoDBProperty("scheduledAppointment")]
        public string? ScheduledAppointment { get; set; } = null;

        [DynamoDBProperty("nameEventProvider")]
        public string? NameEventProvider { get; set; } = null;

        [DynamoDBProperty("typeEventProvider")]
        public string? TypeEventProvider { get; set; } = null;

        [DynamoDBProperty("guaranteePayment")]
        public GuaranteePayment? GuaranteePayment { get; set; }

    }
}
