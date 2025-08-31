using Amazon.DynamoDBv2.DataModel;
using EventServices.Common;

namespace EventServices.EventFirstContact.Domain.Entities
{
    [DynamoDBTable("EventDraft")]

    public class EventDetails
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

        [DynamoDBProperty("descriptionEventDetails")]
        public string? DescriptionEventDetails { get; set; } = null;

        [DynamoDBProperty("priorityEventDetails")]
        public Information PriorityEventDetails { get; set; } = new Information();

        [DynamoDBProperty("categorieEventDetails")]
        public Information CategorieEventDetails { get; set; } = new Information();

        [DynamoDBProperty("typeEventDetails")]
        public Information TypeEventDetails { get; set; } = new Information();

        [DynamoDBProperty("coverageVoucherEventDetails")]
        public string? CoverageEventDetails { get; set; } = null;

        [DynamoDBProperty("coverageDetailVoucherEventDetails")]
        public string? CoverageDetailVoucherEventDetails { get; set; } = null;

        [DynamoDBProperty("requireProviderEventDetails")]
        public bool? RequireProviderEventDetails { get; set; } = null;

        [DynamoDBProperty("providerEventDetails")]
        public string? SelectedProviderEventDetails { get; set; } = null;

        [DynamoDBProperty("requirePhoneMedicalConsultationEventDetails")]
        public bool? RequirePhoneMedicalConsultationEventDetails { get; set; } = null;

        [DynamoDBProperty("requireReviewAssistsTeamEventDetails")]
        public bool? RequireReviewAssistsTeamEventDetails { get; set; } = null;

        [DynamoDBProperty("reviewAssistsTeamEventDetails")]
        public string? SelectedReviewAssistsTeamEventDetails { get; set; } = null;

        [DynamoDBProperty("reviewCashBackTeamEventDetails")]
        public bool? RequireReviewCashBackTeamEventDetails { get; set; } = null;

        [DynamoDBProperty("occurredDate")]
        public string? OccurredDate { get; set; }

        [DynamoDBProperty("returnDate")]
        public string? ReturnDate { get; set; }

        [DynamoDBProperty("travelPurpose")]
        public string TravelPurposeValue { get; set; }

        [DynamoDBIgnore]
        public EnumReasonTravel? TravelPurpose
        {
            get => string.IsNullOrEmpty(TravelPurposeValue)
                ? null
                : Enum.Parse<EnumReasonTravel>(TravelPurposeValue);
            set => TravelPurposeValue = value?.ToString();
        }
    }
}
