using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventServices.Domain.Entities
{
    public class EventProvider
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int EventProviderStatusId { get; set; }
        public string ProviderId { get; set; } = string.Empty;
        public string ProviderLocationId { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string NearTo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ScheduledAppointment { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? NameProvider { get; set; } = string.Empty;
        public string? TypeProvider { get; set; } = string.Empty;
        public string? EmailProvider { get; set; }
        public string? PhoneProvider { get; set; }
        public string? ExternalData { get; set; }
        public string? ExternalRequestId { get; set; } 
        public int AssistanceSubTypeId { get; set; }
        public string? DiagnosisIcd { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;



        [ForeignKey(nameof(EventProviderStatusId))]
        [InverseProperty(nameof(EventProviderStatus.EventProviders))]
        public EventProviderStatus? EventProviderNavigation { get; set; }

        public string? EventProvider_Name => this.EventProviderNavigation?.Name;

        [ForeignKey(nameof(AssistanceSubTypeId))]
        [InverseProperty(nameof(GeneralType.EventProviders))]
        public GeneralType? GeneralTypeNavigation { get; set; }

        public string? GeneralType_Name => this.GeneralTypeNavigation?.Code;

        // Llaves foráneas
        public Event Event { get; set; } = null!;

        public ICollection<PhoneConsultation> PhoneConsultations  { get; set; } = null!;

        public ICollection<GuaranteePayment> GuaranteePayment { get; set; } = null!;

    }

}
