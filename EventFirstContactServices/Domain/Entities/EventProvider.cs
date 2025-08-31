using Amazon.DynamoDBv2.DataModel;

namespace EventFirstContactServices.Domain.Entities
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

        [DynamoDBProperty("guaranteePayment")]
        public GuaranteePayment? GuaranteePayment { get; set; }

    }

    public class GuaranteePayment
    {
        [DynamoDBProperty("amountLocal")]
        public decimal? AmountLocal { get; set; } = null;

        [DynamoDBProperty("typeMoney")]
        public string? TypeMoney { get; set; } = null;

        [DynamoDBProperty("amountUsd")]
        public decimal? AmountUsd { get; set; } = null;

        [DynamoDBProperty("exchangeRate")]
        public decimal? ExchangeRate { get; set; } = null;

        [DynamoDBProperty("deductibleAmountLocal")]
        public decimal? DeductibleAmountLocal { get; set; } = null;

        [DynamoDBProperty("deductibleAmountUsd")]
        public decimal? DeductibleAmountUsd { get; set; } = null;
    }
}
