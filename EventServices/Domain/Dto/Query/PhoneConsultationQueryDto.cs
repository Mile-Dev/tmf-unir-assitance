using EventServices.Common;

namespace EventServices.Domain.Dto.Query
{
    public class PhoneConsultationQueryDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int? EventProviderId { get; set; }
        public string PhoneConsultationSk { get; set; } = string.Empty;
        public string? NameDoctor { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? ScheduledEndAt { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
