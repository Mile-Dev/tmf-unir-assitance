using Amazon.DynamoDBv2.DataModel;

namespace IssuanceMokServices.Domain.Entities
{
    [DynamoDBTable("IssuanceMok")]

    public class UploadEntities
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("PK")]
        public required string PartitionKey { get; set; }

        [DynamoDBProperty("issuanceName")]
        public string IssuanceName { get; set; } = string.Empty;

        [DynamoDBProperty("urlDocument")]
        public string UrlDocument { get; set; } = string.Empty;

        [DynamoDBProperty("metadataDocument")]
        public string Metadata { get; set; } = string.Empty;

        [DynamoDBProperty("createdAt")]
        public string CreatedAt { get; set; } = string.Empty;

        [DynamoDBProperty("updatedAt")]
        public string UpdatedAt { get; set; } = string.Empty;

    }
}
