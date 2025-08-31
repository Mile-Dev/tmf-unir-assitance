using System.ComponentModel.DataAnnotations.Schema;

namespace EventServices.Domain.Entities
{
    public class GuaranteePayment
    {
        public int Id { get; set; }
        public int EventProviderId { get; set; }
        public int GuaranteePaymentStatusId { get; set; }        
        public string? TypeMoney { get; set; } 
        public decimal? AmountLocal { get; set; }
        public decimal? AmountUsd { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? DeductibleAmountLocal { get; set; }
        public decimal? DeductibleAmountUsd { get; set; }
        public string? Description { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Llaves foráneas
        [ForeignKey(nameof(EventProviderId))]
        [InverseProperty(nameof(EventProvider.GuaranteePayment))]
        public EventProvider? EventProviderNavigation { get; set; }

        public string? EventProvider_NameProvider => this.EventProviderNavigation?.NameProvider;

        [ForeignKey(nameof(GuaranteePaymentStatusId))]
        [InverseProperty(nameof(Entities.GuaranteePaymentStatus.GuaranteePayment))]
        public GuaranteePaymentStatus? GuaranteePaymentStatus { get; set; }
        public string? GuaranteePaymentStatus_Name => this.GuaranteePaymentStatus?.Name;


    }
}
