
namespace EventFirstContactServices.Domain.Dto.Get
{
    public class EventFirstContactCustomerTripGetDto
    {
        public string? Id { get; set; } = null;

        public string Screen { get; set; } = string.Empty;

        public string? NameCustomerTrip { get; set; } = null;

        public string? LastNameCustomerTrip { get; set; } = null;

        public string? EmailCustomerTrip { get; set; } = null;

        public string? CellPhoneCustomerTrip { get; set; } = null;

        public InformationGetDto TypeIdentificationPhoneCustomerTrip { get; set; } = new InformationGetDto();    

        public string? IdentificationPhoneCustomerTrip { get; set; } = null;

        public string? CountryOfBirthCustomerTrip { get; set; } = null;

        public int GenderCustomerTrip { get; set; }

        public string? BirthDateCustomerTrip { get; set; } = null;
    }
}
