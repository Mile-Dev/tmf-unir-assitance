using Amazon.DynamoDBv2.DataModel;

namespace EventServices.EventFirstContact.Domain.Entities
{
    public class ContactEmergency
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
