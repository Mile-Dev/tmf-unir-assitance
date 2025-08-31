using Amazon.DynamoDBv2.DataModel;

namespace EventServices.EventFirstContact.Domain.Entities
{
    [DynamoDBTable("EventDraft")]
    public class Event
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("PK")]
        public required string PartitionKey { get; set; } = string.Empty;

        [DynamoDBRangeKey]
        [DynamoDBProperty("SK")]
        public required string ClasificationKey { get; set; } = string.Empty;

        [DynamoDBProperty("nameVoucher")]
        public string? NameVoucher { get; set; } = null;

        [DynamoDBProperty("plan")]
        public string? Plan { get; set; } = null;

        [DynamoDBProperty("dateOfIssue")]
        public string? DateOfIssue { get; set; } = null;

        [DynamoDBProperty("startDate")]
        public string? StartDate { get; set; } = null;

        [DynamoDBProperty("endDate")]
        public string? EndDate { get; set; } = null;

        [DynamoDBProperty("issueName")]
        public string? IssueName { get; set; } = null;

        [DynamoDBProperty("idVoucherStatus")]
        public Information IdVoucherStatus { get; set; } = new Information();

        [DynamoDBProperty("destination")]
        public string? Destination { get; set; } = null;

        [DynamoDBProperty("isCoPayment")]
        public bool IsCoPayment { get; set; } = false;

        [DynamoDBProperty("description")]
        public string? Description { get; set; } = null;

        [DynamoDBProperty("createdAt")]
        public string? CreatedAt { get; set; } = null;

        [DynamoDBProperty("updatedAt")]
        public string? UpdatedAt { get; set; } = null;
    }
}
