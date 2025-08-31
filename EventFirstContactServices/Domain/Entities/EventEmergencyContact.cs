using Amazon.DynamoDBv2.DataModel;

namespace EventFirstContactServices.Domain.Entities
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
        public List<EmergencyContact>? ListEmergencyContactEvent { get; set; } = null;

    }

    public class EmergencyContact
    {
        [DynamoDBProperty("idEmergencyContact")]
        public string? IdEmergencyContact { get; set; } = null;

        [DynamoDBProperty("nameEmergencyContact")]
        public string? NameEmergencyContact { get; set; } = null;

        [DynamoDBProperty("lastnameEmergencyContact")]
        public string? LastNameEmergencyContact { get; set; } = null;

        [DynamoDBProperty("PhoneEmergencyContact")]
        public string? PhoneEmergencyContact { get; set; } = null;

        [DynamoDBProperty("emailEmergencyContact")]
        public string? EmailEmergencyContact { get; set; } = null;

        [DynamoDBProperty("mainPersonEmergencyContact")]
        public bool? MainPersonEmergencyContact { get; set; } = null;
    }

}
