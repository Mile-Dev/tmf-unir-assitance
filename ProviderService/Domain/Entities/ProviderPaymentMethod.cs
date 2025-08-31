using Amazon.DynamoDBv2.DataModel;

namespace ProviderService.Domain.Entities
{
    [DynamoDBTable("ProviderData")]

    public class ProviderPaymentMethod
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("PK")]
        public required string PartitionKey { get; init; }

        [DynamoDBRangeKey]
        [DynamoDBProperty("SK")]
        public required string ClasificationKey { get; init; }

        [DynamoDBProperty("idProvider")]
        public string IdProvider { get; init; } = string.Empty;

        [DynamoDBProperty("idPayment")]
        public string IdPayment { get; set; } = string.Empty;

        [DynamoDBProperty("isActivePayment")]
        public bool? IsActivePayment { get; set; } = null;

        [DynamoDBProperty("typePayment")]
        public string TypePayment { get; set; } = string.Empty;

        [DynamoDBProperty("listData")]
        public List<ListData> ListData { get; set; } = [];

        [DynamoDBProperty("details")]
        public string Details { get; set; } = string.Empty;

        [DynamoDBProperty("createdAt")]
        public string CreatedAt { get; set; } = string.Empty;

        [DynamoDBProperty("updatedAt")]
        public string UpdatedAt { get; set; } = string.Empty;
    }

}
