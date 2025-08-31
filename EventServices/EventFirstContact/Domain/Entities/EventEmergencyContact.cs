using Amazon.DynamoDBv2.DataModel;

namespace EventServices.EventFirstContact.Domain.Entities
{
    [DynamoDBTable("EventDraft")]

    public class EventEmergencyContact
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

        [DynamoDBProperty("listEmergencyContact")]
        public List<ContactEmergency>? ListEmergencyContactEvent { get; set; } = null;

    }
}
