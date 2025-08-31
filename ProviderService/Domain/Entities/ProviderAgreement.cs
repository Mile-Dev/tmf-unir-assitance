using Amazon.DynamoDBv2.DataModel;

namespace ProviderService.Domain.Entities
{
    [DynamoDBTable("ProviderData")]

    public class ProviderAgreement
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("PK")]
        public required string PartitionKey { get; init; }

        [DynamoDBRangeKey]
        [DynamoDBProperty("SK")]
        public required string ClasificationKey { get; init; }

        [DynamoDBProperty("idProvider")]
        public string IdProvider { get; set; } = string.Empty;

        [DynamoDBProperty("idAgreement")]
        public string IdAgreement { get; set; } = string.Empty;

        [DynamoDBProperty("endValidity")]
        public string EndValidity { get; set; } = string.Empty;

        [DynamoDBProperty("startValidity")]
        public string StartValidity { get; set; } = string.Empty;

        [DynamoDBProperty("urlAttach")]
        public List<string> UrlAttach { get; set; } = [];

        [DynamoDBProperty("createdAt")]
        public string CreatedAt { get; set; } = string.Empty;

        [DynamoDBProperty("updatedAt")]
        public string UpdatedAt { get; set; } = string.Empty;
    }
}
