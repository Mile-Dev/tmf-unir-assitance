using AutoMapper;
using EventServices.Common;
using EventServices.Domain.Dto.Query;
using EventServices.Domain.Entities;
using EventServices.EventFirstContact.Domain.Dto.Create.DynamoDb;
using EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb;
using EmergencyContactDynamoDb = EventServices.EventFirstContact.Domain.Entities.ContactEmergency;
using EventCustomerTripDynamoDb = EventServices.EventFirstContact.Domain.Entities.EventCustomerTrip;
using EventDetailsDynamoDb = EventServices.EventFirstContact.Domain.Entities.EventDetails;
using EventDynamoDb = EventServices.EventFirstContact.Domain.Entities.Event;
using EventEmergencyContactDynamoDb = EventServices.EventFirstContact.Domain.Entities.EventEmergencyContact;
using EventLocationDynamoDb = EventServices.EventFirstContact.Domain.Entities.EventLocation;
using EventProviderDynamoDb = EventServices.EventFirstContact.Domain.Entities.EventProvider;
using GuaranteePaymentDynamoDb = EventServices.EventFirstContact.Domain.Entities.GuaranteePayment;
using InfomationDynamoDb = EventServices.EventFirstContact.Domain.Entities.Information;

namespace EventServices.EventFirstContact.Domain.Mapping
{
    public class AutomapperDynamoProfile : Profile
    {
        public AutomapperDynamoProfile()
        {
            CreateMap<GuaranteePaymentDynamoDb, GuaranteePaymentCreatedDto>()
       .ReverseMap();

            CreateMap<GuaranteePaymentDynamoDb, GuaranteePaymentQueryDto>()
            .ReverseMap();

            CreateMap<ViewGuaranteesPaymentEventProvider, ViewGuaranteesPaymentEventProviderGetDto>()
            .ReverseMap();

            CreateMap<InfomationDynamoDb, InformationDto>()
            .ReverseMap();

            CreateMap<InfomationDynamoDb, InformationQueryDto>()
            .ReverseMap();

            CreateMap<EventDynamoDb, EventFirstContactDto>()
               .ReverseMap();

            CreateMap<EventDynamoDb, ResponseEventVoucherDto>()
            .ReverseMap();

            CreateMap<EventCustomerTripDynamoDb, EventFirstContactDto>()
               .ReverseMap();

            CreateMap<EventCustomerTripDynamoDb, ResponseEventFirstContactCustomerTripDto>()
            .ReverseMap();

            CreateMap<EventFirstContactDto, EventDetailsDynamoDb>()
                .ForMember(dest => dest.TravelPurpose, opt => opt.MapFrom(src => EnumExtensions.FromDescription<EnumReasonTravel>(src.TravelPurpose)))
            .ReverseMap();

            CreateMap<EventDetailsDynamoDb, ResponseEventFirstContactDetailsDto>()
                 .ForMember(dest => dest.TravelPurpose, opt => opt.MapFrom(src => src.TravelPurpose.HasValue ? EnumExtensions.GetDescription(src.TravelPurpose.Value) : null))
            .ReverseMap();

            CreateMap<EventEmergencyContactDynamoDb, EventFirstContactDto>()
                .ReverseMap();

            CreateMap<EventEmergencyContactDynamoDb, ResponseEventFirstContactEmergencyContactDto>()
                .ReverseMap();

            CreateMap<EmergencyContactDynamoDb, EmergencyContactCreatedDto>()
                .ReverseMap();

            CreateMap<EmergencyContactDynamoDb, EmergencyContactQueryDto>()
             .ReverseMap();

            CreateMap<EventLocationDynamoDb, EventFirstContactDto>()
                .ReverseMap();

            CreateMap<EventLocationDynamoDb, ResponseEventFirstContactLocationDto>()
                .ReverseMap();

            CreateMap<EventProviderDynamoDb, EventFirstContactDto>()
                .ReverseMap();

            CreateMap<EventProviderDynamoDb, ResponseEventFirstContactProviderDto>()
                .ReverseMap();
        }
    }
}
