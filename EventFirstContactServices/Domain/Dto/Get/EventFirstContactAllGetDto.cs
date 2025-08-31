namespace EventFirstContactServices.Domain.Dto.Get
{
    public class EventFirstContactAllGetDto
    {
        public string Id { get; set; } = string.Empty;

        public EventGetDto Event { get; set; } = new EventGetDto();

        public EventFirstContactCustomerTripGetDto EventCustomerTrip { get; set; } = new EventFirstContactCustomerTripGetDto();

        public EventFirstContactLocationGetDto EventLocation { get; set; } = new EventFirstContactLocationGetDto();

        public EventFirstContactEmergencyContactGetDto EventEmergencyContact { get; set; } = new EventFirstContactEmergencyContactGetDto();

        public EventFirstContactDetailsGetDto EventDetails { get; set; } = new EventFirstContactDetailsGetDto();
              
        public EventFirstContactProviderGetDto EventProvider { get; set; } = new EventFirstContactProviderGetDto();

    }
}
