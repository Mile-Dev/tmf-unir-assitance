using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace EventFirstContactServices.Domain.Dto.Get
{
    public class EventFirstContactDetailsGetDto
    {
        public string? Id { get; set; } = null;

        public string Screen { get; set; } = string.Empty;

        public string? DescriptionEventDetails { get; set; } = null;

        public InformationGetDto PriorityEventDetails { get; set; } = new InformationGetDto();

        public InformationGetDto CategorieEventDetails { get; set; } = new InformationGetDto();

        public InformationGetDto TypeEventDetails { get; set; } = new InformationGetDto();

        public string? CoverageEventDetails { get; set; } = null;

        public string? CoverageDetailVoucherEventDetails { get; set; } = null;

        public bool? RequireProviderEventDetails { get; set; } = null;

        public string? SelectedProviderEventDetails { get; set; } = null;

        public bool? RequirePhoneMedicalConsultationEventDetails { get; set; } = null;

        public bool? RequireReviewAssistsTeamEventDetails { get; set; } = null;

        public string? SelectedReviewAssistsTeamEventDetails { get; set; } = null;
      
        public bool? RequireReviewCashBackTeamEventDetails { get; set; } = null;
    }
}
