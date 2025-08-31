namespace EventFirstContactServices.Domain.Dto.Get
{
    public class EventFirstContactProviderGetDto
    {
        public string? Id { get; set; } = null;

        public string Screen { get; set; } = string.Empty;
    
        public string IdProvider { get; set; } = string.Empty;

        public string IdLocation { get; set; } = string.Empty;

        public string? CountryEventProvider { get; set; } = null;

        public string? CityEventProvider { get; set; } = null;

        public string? NearOfEventProvider { get; set; } = null;

        public string? AddressEventProvider { get; set; } = null;

        public string? InformationEventProvider { get; set; } = null;

        public string? GpsEventProvider { get; set; } = null;

        public GuaranteePaymentGetDto? GuaranteePayment { get; set; }

    }

    public class GuaranteePaymentGetDto
    {
        public decimal? AmountLocal { get; set; } = null;
        public string? TypeMoney { get; set; } = null;
        public decimal? AmountUsd { get; set; } = null;
        public decimal? ExchangeRate { get; set; } = null;
        public decimal? DeductibleAmountLocal { get; set; } = null;
        public decimal? DeductibleAmountUsd { get; set; } = null;
    }
}
