using EventServices.Domain.Dto.Query;

namespace EventServices.Domain.Dto
{
    public class RequestEvent
    {
        public int? Client { get; set; }
        public RequestEventVoucherDto EventVoucher { get; set; } = new RequestEventVoucherDto();

        public RequestEventCustomerTripDto EventCustomerTrip { get; set; } = new RequestEventCustomerTripDto();

        public RequestEventLocationDto EventLocation { get; set; } = new RequestEventLocationDto();

        public RequestEventEmergencyContactDto EventEmergencyContact { get; set; } = new RequestEventEmergencyContactDto();

        public RequestEventDetailsDto EventDetails { get; set; } = new RequestEventDetailsDto();

        public RequestMok? FieldsAditionalsMok { get; set; } = new RequestMok();

    }
}
