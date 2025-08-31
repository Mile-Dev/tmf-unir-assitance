namespace EventServices.Domain.Dto.Query
{
    public class RequestEventCustomerTripDto
    {
        public string NameCustomerTrip { get; set; } = string.Empty;

        public string LastNameCustomerTrip { get; set; } = string.Empty;

        public string EmailCustomerTrip { get; set; } = string.Empty;

        public string PhoneCustomerTrip { get; set; } = string.Empty;

        public int TypeIdentificationCustomerTrip { get; set; } 

        public string IdentificationCustomerTrip { get; set; } = string.Empty;

        public string CountryOfBirthCustomerTrip { get; set; } = string.Empty;

        public int GenderCustomerTrip { get; set; }

        public string BirthDateCustomerTrip { get; set; } = string.Empty;
    }
}
