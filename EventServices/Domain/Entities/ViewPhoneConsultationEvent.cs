namespace EventServices.Domain.Entities
{
    public class ViewPhoneConsultationEvent
    {
        public int Id { get; set; }

        public int IdPhoneConsultation { get; set; }

        public string CodeEvent { get; set; } = string.Empty;

        public int VoucherStatusId { get; set; }
        
        public int EventStatusId { get; set; }
       
        public string Voucher { get; set; } = string.Empty;
     
        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string EventType { get; set; } = string.Empty;

        public string EventStatus { get; set; } = string.Empty;

        public DateTime ScheduledAt { get; set; }

        public DateTime? ScheduledEndAt { get; set; } = null;

        public string? StatusLastPhoneConsultation { get; set; } = null;

        public string LastDoctorAssigned { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   
    }
}
