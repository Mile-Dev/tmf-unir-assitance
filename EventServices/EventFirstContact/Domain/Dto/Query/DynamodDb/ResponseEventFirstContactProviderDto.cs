namespace EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb
{
    public class ResponseEventFirstContactProviderDto
    {

        public string Screen { get; set; } = string.Empty;

        public string IdProvider { get; set; } = string.Empty;

        public string IdLocation { get; set; } = string.Empty;

        public string? CountryEventProvider { get; set; } = null;

        public string? CityEventProvider { get; set; } = null;

        public string? NearOfEventProvider { get; set; } = null;

        public string? AddressEventProvider { get; set; } = null;

        public string? InformationEventProvider { get; set; } = null;

        public string? GpsEventProvider { get; set; } = null;

        public string? ScheduledAppointment { get; set; } = null;

        public string? NameEventProvider { get; set; } = null;

        public string? TypeEventProvider { get; set; } = null;


        public GuaranteePaymentQueryDto? GuaranteePayment { get; set; }

    }

}
