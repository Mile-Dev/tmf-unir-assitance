namespace EventServices.Domain.Entities
{
    public class ViewGuaranteesPaymentEventProvider
    {
        public int Id { get; set; }
       
        public int EventId { get; set; }

        public int EventProviderId { get; set; }
        public int StatusEventProviderId { get; set; }
        
        public int GuaranteePaymentStatusId { get; set; }

        public string? NameStatusEventProvider { get; set; } = string.Empty;
       
        public string? NameStatusGuaranteePayment { get; set; } = string.Empty;

        public string? Country { get; set; } = string.Empty;

        public string? City { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;

        public string? NameProvider { get; set; } = string.Empty;

        public string? TypeProvider { get; set; } = string.Empty;

        public string? TypeMoney { get; set; } = string.Empty;

        public decimal? AmountLocal { get; set; }

        public decimal? AmountUsd { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? DeductibleAmountLocal { get; set; }

        public decimal? DeductibleAmountUsd { get; set; }

        public string? Description { get; set; } = null;

        public DateTime? ScheduledAppointment { get; set; }

        public DateTime? EndDate { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
