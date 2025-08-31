using System.ComponentModel.DataAnnotations.Schema;

namespace EventStatusSwitchTempServices.Domain.Entities
{
    public class Events
    {
        public int Id { get; set; }
        public int EventStatusId { get; set; }
        public int GeneralTypesId { get; set; }
        public int VoucherId { get; set; }
        public int CustomerTripId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CountryLocation { get; set; } = string.Empty;
        public string? StateLocation { get; set; } = string.Empty;
        public string? CityLocation { get; set; } = string.Empty;
        public string? NearToLocation { get; set; } = string.Empty;
        public string? AddressLocation { get; set; } = string.Empty;
        public string? InformationLocation { get; set; } = string.Empty;
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Description { get; set; } = string.Empty;
        public int EventPriority { get; set; }
        public bool RequireProvider { get; set; }
        public string ProviderDetails { get; set; } = string.Empty;
        public bool RequirePhoneMedicalConsultation { get; set; }
        public bool RequireReviewAssistsTeam { get; set; }
        public string ReviewAssistsTeamDetails { get; set; } = string.Empty;
        public bool RequireCashBackTeam { get; set; }

        // Llaves foráneas
        [ForeignKey(nameof(EventStatusId))]
        [InverseProperty(nameof(EventStatus.Events))]
        public EventStatus? EventStatusNavigation { get; set; }

        public string? EventStatus_Name => this.EventStatusNavigation?.Name;
    }
}
