namespace EventServices.Domain.Dto.Create
{
    public class GuaranteePaymentDto
    {
        public int EventProviderId { get; set; }
        public decimal? AmountLocal { get; set; } = null;
        public string? TypeMoney { get; set; } = null;
        public decimal? AmountUsd { get; set; } = null;
        public decimal? ExchangeRate { get; set; } = null;
        public decimal? DeductibleAmountLocal { get; set; } = null;
        public decimal? DeductibleAmountUsd { get; set; } = null;
        public string? Description { get; set; } = null;

    }
}
