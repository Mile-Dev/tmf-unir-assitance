namespace EventServices.Domain.Dto.Create
{
    public class PhoneConsultationDto
    {
        public int EventProviderId { get; set; }
        public string? User { get; set; }
        public string? NameDoctor { get; set; }
        public string? EmailDoctor { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}
