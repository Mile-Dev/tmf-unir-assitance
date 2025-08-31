namespace EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb
{
    public class ResponseEventFirstContactCustomerTripDto
    {

        public string Screen { get; set; } = string.Empty;

        public string NameCustomerTrip { get; set; } = string.Empty;

        public string LastNameCustomerTrip { get; set; } = string.Empty;

        public string EmailCustomerTrip { get; set; } = string.Empty;

        public string CellPhoneCustomerTrip { get; set; } = string.Empty;

        public InformationQueryDto TypeIdentificationPhoneCustomerTrip { get; set; } = new InformationQueryDto();

        public string IdentificationPhoneCustomerTrip { get; set; } = string.Empty;

        public string CountryOfBirthCustomerTrip { get; set; } = string.Empty;

        public int GenderCustomerTrip { get; set; }

        public string BirthDateCustomerTrip { get; set; } = string.Empty;
    }
}
