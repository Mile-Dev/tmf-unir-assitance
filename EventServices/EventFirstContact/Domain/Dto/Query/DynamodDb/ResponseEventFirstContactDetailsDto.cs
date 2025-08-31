namespace EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb
{
    public class ResponseEventFirstContactDetailsDto
    {

        public string Screen { get; set; } = string.Empty;

        public string? DescriptionEventDetails { get; set; } = null;

        public InformationQueryDto PriorityEventDetails { get; set; } = new InformationQueryDto();

        public InformationQueryDto CategorieEventDetails { get; set; } = new InformationQueryDto();

        public InformationQueryDto TypeEventDetails { get; set; } = new InformationQueryDto();

        public string? CoverageEventDetails { get; set; } = null;

        public string? CoverageDetailVoucherEventDetails { get; set; } = null;

        public bool RequireProviderEventDetails { get; set; } = false;

        public string? SelectedProviderEventDetails { get; set; } = null;

        public bool RequirePhoneMedicalConsultationEventDetails { get; set; } = false;

        public bool RequireReviewAssistsTeamEventDetails { get; set; } = false;

        public string? SelectedReviewAssistsTeamEventDetails { get; set; } = null;

        public bool RequireReviewCashBackTeamEventDetails { get; set; } = false;
     
        public string? OccurredDate { get; set; }

        public string? ReturnDate { get; set; }

        public string? TravelPurpose { get; set; }
    }
}
