namespace EventServices.Domain.Dto.Query
{
    public record GuaranteePaymentDto
    {
        public int Id { get; set; }
        public int EventProviderId { get; set; }
        public int GuaranteePaymentStatusId { get; set; }
        public string? EventProviderNameProvider { get; set; }
        public string? GuaranteePaymentStatusName{ get; set; }
        public string TypeMoney { get; set; } = string.Empty;
        public decimal AmountLocal { get; set; }
        public decimal AmountUsd { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal DeductibleAmountLocal { get; set; }
        public decimal DeductibleAmountUsd { get; set; }
        public string? Description { get; set; } = null;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
