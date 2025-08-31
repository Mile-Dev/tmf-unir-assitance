using Amazon.DynamoDBv2.DataModel;

namespace EventServices.EventFirstContact.Domain.Entities
{
    [DynamoDBTable("EventDraft")]

    public class EventCustomerTrip
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

        [DynamoDBProperty("nameCustomerTrip")]
        public string? NameCustomerTrip { get; set; } = null;

        [DynamoDBProperty("lastnameCustomerTrip")]
        public string? LastNameCustomerTrip { get; set; } = null;

        [DynamoDBProperty("emailCustomerTrip")]
        public string? EmailCustomerTrip { get; set; } = null;

        [DynamoDBProperty("cellPhoneCustomerTrip")]
        public string? CellPhoneCustomerTrip { get; set; } = null;

        [DynamoDBProperty("typeIdentificationCustomerTrip")]
        public Information TypeIdentificationPhoneCustomerTrip { get; set; } = new Information();

        [DynamoDBProperty("identificationCustomerTrip")]
        public string? IdentificationPhoneCustomerTrip { get; set; } = null;

        [DynamoDBProperty("countryOfBirthCustomerTrip")]
        public string? CountryOfBirthCustomerTrip { get; set; } = null;

        [DynamoDBProperty("genderCustomerTrip")]
        public int GenderCustomerTrip { get; set; }

        [DynamoDBProperty("birthDateCustomerTrip")]
        public string? BirthDateCustomerTrip { get; set; } = null;

    }
}
