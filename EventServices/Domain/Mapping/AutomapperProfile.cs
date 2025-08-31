using AutoMapper;
using EventServices.Common;
using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Dto.Query;
using EventServices.Domain.Entities;
using ContactEmergency = EventServices.Domain.Entities.ContactEmergency;
using Event = EventServices.Domain.Entities.Event;
using EventProvider = EventServices.Domain.Entities.EventProvider;
using GuaranteePayment = EventServices.Domain.Entities.GuaranteePayment;


namespace EventServices.Domain.Mapping;
public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<RequestEventVoucherDto, Voucher>()
         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NameVoucher))
         .ForMember(dest => dest.VoucherStatusId, opt => opt.MapFrom(src => src.VoucherStatusId))
         .ForMember(dest => dest.DateOfIssue, opt => opt.MapFrom(src => DateTime.Parse(src.DateOfIssue)))
         .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.Parse(src.StartDate)))
         .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateTime.Parse(src.EndDate)))
         .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
         .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
         .ReverseMap();

        CreateMap<RequestEventCustomerTripDto, CustomerTrip>()
        .ForMember(dest => dest.Names, opt => opt.MapFrom(src => src.NameCustomerTrip))
        .ForMember(dest => dest.LastNames, opt => opt.MapFrom(src => src.LastNameCustomerTrip))
        .ForMember(dest => dest.CountryOfBirth, opt => opt.MapFrom(src => src.CountryOfBirthCustomerTrip))
        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GenderCustomerTrip))
        .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateTime.Parse(src.BirthDateCustomerTrip)))
        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
        .ReverseMap();

        CreateMap<RequestEventDetailsDto, Event>()
         .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionEvent))
         .ForMember(dest => dest.EventPriority, opt => opt.MapFrom(src => src.PriorityIdEvent))
         .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
         .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
         .ForMember(dest => dest.TravelPurpose, opt => opt.MapFrom(src => EnumExtensions.FromDescription<EnumReasonTravel>(src.TravelPurpose)))
         .ReverseMap();

        CreateMap<EmergencyContactDto, ContactEmergency>()
         .ForMember(dest => dest.Names, opt => opt.MapFrom(src => src.NameEmergencyContact))
         .ForMember(dest => dest.LastNames, opt => opt.MapFrom(src => src.LastNameEmergencyContact))
         .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailEmergencyContact))
         .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneEmergencyContact))
         .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.MainPersonEmergencyContact))
         .ReverseMap();

        CreateMap<EmergencyContactGetDto, ContactEmergency>()
         .ForMember(dest => dest.Names, opt => opt.MapFrom(src => src.NameEmergencyContact))
         .ForMember(dest => dest.LastNames, opt => opt.MapFrom(src => src.LastNameEmergencyContact))
         .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailEmergencyContact))
         .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneEmergencyContact))
         .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.MainPersonEmergencyContact))
         .ReverseMap();

        CreateMap<GuaranteePayment, Dto.Query.GuaranteePaymentDto>()
            .ReverseMap();

        CreateMap<GuaranteePayment, Dto.Create.GuaranteePaymentDto>()
            .ReverseMap();

        CreateMap<Category, CategoriesQueryDto>()
         .ReverseMap();

        CreateMap<Category, CategoriesDto>()
         .ReverseMap();

        CreateMap<GeneralType, GeneralTypesQueryDto>()
         .ReverseMap();

        CreateMap<EventStatus, EventStatusQueryDto>()
         .ReverseMap();

        CreateMap<VoucherStatus, VoucherStatusQueryDto>()
        .ReverseMap();

        CreateMap<EventCoverage, EventCoveragesDto>()
        .ReverseMap();

        CreateMap<ViewEvent, ViewEventsGetDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name}{" "}{src.LastName}"))
        .ReverseMap();

        CreateMap<ViewEventDetail, ViewEventDetailsGetDto>()
       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ReverseMap();

        CreateMap<ViewPhoneConsultationEvent, ViewPhoneConsultationEventGetDto>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ReverseMap();

        CreateMap(typeof(PaginatedData<>), typeof(PaginatedDataQueryDto))
         .ForMember("Data", opt => opt.MapFrom("Data"))
         .ForMember("TotalCount", opt => opt.MapFrom("TotalCount"));

        CreateMap<EventProvider, Dto.Create.EventProviderDto>()
         .ReverseMap();

        CreateMap<EventProvider, Dto.Query.EventProviderDto>()
            .ForMember(dest => dest.EventProviderStatus, opt => opt.MapFrom(src => src.EventProvider_Name))
          .ReverseMap();

        CreateMap<PhoneConsultation, PhoneConsultationQueryDto>()
          .ReverseMap();

        CreateMap<PhoneConsultation, PhoneConsultationDto>()
          .ReverseMap();

        CreateMap<Document, DocumentCreatedDto>()
            .ReverseMap();

        CreateMap<Document, DocumentGetDto>()
          .ReverseMap();
    }
}

