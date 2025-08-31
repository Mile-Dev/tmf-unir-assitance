namespace EventServices.Domain.Dto
{
    public class RequestUpdatedEvent
    {
        public int VoucherStatusId { get; set; }

        public int EventStatusId { get; set; } 

        public int TypeAssistanceIdEvent { get; set; }

        public int GenderCustomerTrip { get; set; } 

        public string BirthDateCustomerTrip { get; set; } = string.Empty; 

        public string NameCustomerTrip { get; set; } = string.Empty; 

        public string LastNameCustomerTrip { get; set; } = string.Empty; 

        public string IdentificationCustomerTrip { get; set; } = string.Empty; 

        public string EmailCustomerTrip { get; set; } = string.Empty;  

        public string PhoneCustomerTrip { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;


    }
}
