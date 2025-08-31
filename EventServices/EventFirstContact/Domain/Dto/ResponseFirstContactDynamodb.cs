using EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb;

namespace EventServices.EventFirstContact.Domain.Dto
{
    public class ResponseFirstContactDynamodb
    {
        public string Id { get; set; } = string.Empty;

        public ResponseEventVoucherDto Event { get; set; } = new ResponseEventVoucherDto();

        public ResponseEventFirstContactCustomerTripDto EventCustomerTrip { get; set; } = new ResponseEventFirstContactCustomerTripDto();

        public ResponseEventFirstContactLocationDto EventLocation { get; set; } = new ResponseEventFirstContactLocationDto();

        public ResponseEventFirstContactEmergencyContactDto EventEmergencyContact { get; set; } = new ResponseEventFirstContactEmergencyContactDto();

        public ResponseEventFirstContactDetailsDto EventDetails { get; set; } = new ResponseEventFirstContactDetailsDto();

        public ResponseEventFirstContactProviderDto EventProvider { get; set; } = new ResponseEventFirstContactProviderDto();
    }
}
