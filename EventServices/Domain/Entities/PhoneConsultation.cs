using System.ComponentModel.DataAnnotations.Schema;

namespace EventServices.Domain.Entities
{
    public class PhoneConsultation
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int? EventProviderId { get; set; }
        public string PhoneConsultationSk { get; set; } = string.Empty;
        public string? User { get; set; }
        public string? NameDoctor { get; set; }
        public string? EmailDoctor { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? ScheduledEndAt { get; set; }
        public string? Status { get; set; }
        public string? DiagnosisIcd { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(EventProviderId))]
        [InverseProperty(nameof(EventProvider.PhoneConsultations))]
        public EventProvider? PhoneConsultationNavigation { get; set; }
    }
}
