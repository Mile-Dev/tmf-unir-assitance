namespace EventServices.Domain.Dto.Create
{
    public class EventProviderDto
    {
        public int EventId { get; set; }
        public string ProviderId { get; set; } = string.Empty;
        public string ProviderLocationId { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string NearTo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? NameProvider { get; set; }
        public string? TypeProvider { get; set; }
        public string? EmailProvider { get; set; }
        public string? PhoneProvider { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ScheduledAppointment { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ExternalRequestId { get; set; } = null;
        public int AssistanceSubTypeId { get; set; }
        public string? DiagnosisIcd { get; set; } = null;

    }
}
