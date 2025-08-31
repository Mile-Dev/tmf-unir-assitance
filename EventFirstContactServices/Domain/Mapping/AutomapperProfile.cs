using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using EventFirstContactServices.Domain.Dto;
using EventFirstContactServices.Domain.Dto.Get;
using EventFirstContactServices.Domain.Entities;

namespace EventFirstContactServices.Domain.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Event, EventFirstContactCreateDto>()
                .ReverseMap();

            CreateMap<EventCustomerTrip, EventFirstContactCreateDto>()
                .ReverseMap();

            CreateMap<EventDetails, EventFirstContactCreateDto>()
                .ReverseMap();

            CreateMap<EventLocation, EventFirstContactCreateDto>()
               .ReverseMap();

            CreateMap<EventProvider, EventFirstContactCreateDto>()
               .ReverseMap();

            CreateMap<EventEmergencyContact, EventFirstContactCreateDto>()
               .ReverseMap();

            CreateMap<EmergencyContact, EmergencyContactDto>()
                .ReverseMap();

            CreateMap<Event, EventGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PartitionKey))
                .ForMember(dest => dest.Screen, opt => opt.MapFrom(src => src.ClasificationKey));


            CreateMap<EventFirstContactCreateDto, EventGetDto>();

            CreateMap<Document, EventGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContainsKey("PK") ? src["PK"].AsString() : string.Empty))
                .ForMember(dest => dest.Screen, opt => opt.MapFrom(src => src.ContainsKey("SK") ? src["SK"].AsString() : string.Empty))
                .ForMember(dest => dest.NameVoucher, opt => opt.MapFrom(src => src.ContainsKey("nameVoucher") ? src["nameVoucher"].AsString() : string.Empty))
                .ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.ContainsKey("plan") ? src["plan"].AsString() : string.Empty))
                .ForMember(dest => dest.DateOfIssue, opt => opt.MapFrom(src => src.ContainsKey("dateOfIssue") ? src["dateOfIssue"].AsString() : string.Empty))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.ContainsKey("startDate") ? src["startDate"].AsString() : string.Empty))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.ContainsKey("endDate") ? src["endDate"].AsString() : string.Empty))
                .ForMember(dest => dest.IssueName, opt => opt.MapFrom(src => src.ContainsKey("issueName") ? src["issueName"].AsString() : string.Empty))
                .ForMember(dest => dest.IdVoucherStatus, opt => opt.MapFrom(src => src.ContainsKey("idVoucherStatus") ? src["idVoucherStatus"].AsDocument() : new Document()))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.ContainsKey("destination") ? src["destination"].AsString() : string.Empty))
                .ForMember(dest => dest.IsCoPayment, opt => opt.MapFrom(src => src.ContainsKey("isCoPayment") ? src["isCoPayment"].AsBoolean() : false))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ContainsKey("description") ? src["description"].AsString() : string.Empty));

            CreateMap<Document, EventFirstContactCustomerTripGetDto>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContainsKey("PK") ? src["PK"].AsString() : string.Empty))
                 .ForMember(dest => dest.Screen, opt => opt.MapFrom(src => src.ContainsKey("SK") ? src["SK"].AsString() : string.Empty))
                 .ForMember(dest => dest.NameCustomerTrip, opt => opt.MapFrom(src => src.ContainsKey("nameCustomerTrip") ? src["nameCustomerTrip"].AsString() : string.Empty))
                 .ForMember(dest => dest.LastNameCustomerTrip, opt => opt.MapFrom(src => src.ContainsKey("lastnameCustomerTrip") ? src["lastnameCustomerTrip"].AsString() : string.Empty))
                 .ForMember(dest => dest.EmailCustomerTrip, opt => opt.MapFrom(src => src.ContainsKey("emailCustomerTrip") ? src["emailCustomerTrip"].AsString() : string.Empty))
                 .ForMember(dest => dest.CellPhoneCustomerTrip, opt => opt.MapFrom(src => src.ContainsKey("cellPhoneCustomerTrip") ? src["cellPhoneCustomerTrip"].AsString() : string.Empty))
                 .ForMember(dest => dest.TypeIdentificationPhoneCustomerTrip, opt => opt.MapFrom(src => src.ContainsKey("typeIdentificationCustomerTrip") ? src["typeIdentificationCustomerTrip"].AsDocument() : new Document()))
                 .ForMember(dest => dest.IdentificationPhoneCustomerTrip, opt => opt.MapFrom(src => src.ContainsKey("identificationCustomerTrip") ? src["identificationCustomerTrip"].AsString() : string.Empty))
                 .ForMember(dest => dest.CountryOfBirthCustomerTrip, opt => opt.MapFrom(src => src.ContainsKey("countryOfBirthCustomerTrip") ? src["countryOfBirthCustomerTrip"].AsString() : string.Empty))
                 .ForMember(dest => dest.GenderCustomerTrip, opt => opt.MapFrom(src => src.ContainsKey("genderCustomerTrip") ? src["genderCustomerTrip"].AsString() : string.Empty))
                 .ForMember(dest => dest.BirthDateCustomerTrip, opt => opt.MapFrom(src => src.ContainsKey("birthDateCustomerTrip") ? src["birthDateCustomerTrip"].AsString() : string.Empty));

            CreateMap<Document, EventFirstContactDetailsGetDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContainsKey("PK") ? src["PK"].AsString() : string.Empty))
               .ForMember(dest => dest.Screen, opt => opt.MapFrom(src => src.ContainsKey("SK") ? src["SK"].AsString() : string.Empty))
               .ForMember(dest => dest.DescriptionEventDetails, opt => opt.MapFrom(src => src.ContainsKey("descriptionEventDetails") ? src["descriptionEventDetails"].AsString() : string.Empty))
               .ForMember(dest => dest.PriorityEventDetails, opt => opt.MapFrom(src => src.ContainsKey("priorityEventDetails") ? src["priorityEventDetails"].AsDocument() : new Document()))
               .ForMember(dest => dest.CoverageEventDetails, opt => opt.MapFrom(src => src.ContainsKey("coverageVoucherEventDetails") ? src["coverageVoucherEventDetails"].AsString() : string.Empty))
               .ForMember(dest => dest.CoverageDetailVoucherEventDetails, opt => opt.MapFrom(src => src.ContainsKey("coverageDetailVoucherEventDetails") ? src["coverageDetailVoucherEventDetails"].AsString() : string.Empty))
               .ForMember(dest => dest.RequireProviderEventDetails, opt => opt.MapFrom(src => src.ContainsKey("requireProviderEventDetails") ? src["requireProviderEventDetails"].AsBoolean() : false))
               .ForMember(dest => dest.SelectedProviderEventDetails, opt => opt.MapFrom(src => src.ContainsKey("providerEventDetails") ? src["providerEventDetails"].AsString() : string.Empty))
               .ForMember(dest => dest.RequirePhoneMedicalConsultationEventDetails, opt => opt.MapFrom(src => src.ContainsKey("requirePhoneMedicalConsultationEventDetails") ? src["requirePhoneMedicalConsultationEventDetails"].AsBoolean() : false))
               .ForMember(dest => dest.RequireReviewAssistsTeamEventDetails, opt => opt.MapFrom(src => src.ContainsKey("requireReviewAssistsTeamEventDetails") ? src["requireReviewAssistsTeamEventDetails"].AsBoolean() : false))
               .ForMember(dest => dest.SelectedReviewAssistsTeamEventDetails, opt => opt.MapFrom(src => src.ContainsKey("reviewAssistsTeamEventDetails") ? src["reviewAssistsTeamEventDetails"].AsString() : string.Empty))
               .ForMember(dest => dest.RequireReviewCashBackTeamEventDetails, opt => opt.MapFrom(src => src.ContainsKey("reviewCashBackTeamEventDetails") ? src["reviewCashBackTeamEventDetails"].AsBoolean() : false))
               .ForMember(dest => dest.CategorieEventDetails, opt => opt.MapFrom(src => src.ContainsKey("categorieEventDetails") ? src["categorieEventDetails"].AsDocument() : new Document()))
               .ForMember(dest => dest.TypeEventDetails, opt => opt.MapFrom(src => src.ContainsKey("typeEventDetails") ? src["typeEventDetails"].AsDocument() : new Document()));


            CreateMap<Document, EventFirstContactEmergencyContactGetDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContainsKey("PK") ? src["PK"].AsString() : string.Empty))
               .ForMember(dest => dest.Screen, opt => opt.MapFrom(src => src.ContainsKey("SK") ? src["SK"].AsString() : string.Empty))
              .ForMember(dest => dest.ListEmergencyContactEvent, opt => opt.MapFrom(src => src.ContainsKey("listEmergencyContact") ? src["listEmergencyContact"].AsDynamoDBList() : new List<Document>()));

            CreateMap<Document, EmergencyContactDto>()
                .ForMember(dest => dest.NameEmergencyContact, opt => opt.MapFrom(src => src.ContainsKey("nameEmergencyContact") ? src["nameEmergencyContact"].AsString() : string.Empty))
                .ForMember(dest => dest.LastNameEmergencyContact, opt => opt.MapFrom(src => src.ContainsKey("lastnameEmergencyContact") ? src["lastnameEmergencyContact"].AsString() : string.Empty))
                .ForMember(dest => dest.PhoneEmergencyContact, opt => opt.MapFrom(src => src.ContainsKey("PhoneEmergencyContact") ? src["PhoneEmergencyContact"].AsString() : string.Empty))
                .ForMember(dest => dest.EmailEmergencyContact, opt => opt.MapFrom(src => src.ContainsKey("emailEmergencyContact") ? src["emailEmergencyContact"].AsString() : string.Empty))
                .ForMember(dest => dest.MainPersonEmergencyContact, opt => opt.MapFrom(src => src.ContainsKey("mainPersonEmergencyContact") ? src["mainPersonEmergencyContact"].AsBoolean() : false));

            CreateMap<Document, EventFirstContactLocationGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContainsKey("PK") ? src["PK"].AsString() : string.Empty))
                .ForMember(dest => dest.Screen, opt => opt.MapFrom(src => src.ContainsKey("SK") ? src["SK"].AsString() : string.Empty))
                .ForMember(dest => dest.CountryEventLocation, opt => opt.MapFrom(src => src.ContainsKey("countryEventLocation") ? src["countryEventLocation"].AsString() : string.Empty))
                .ForMember(dest => dest.CityEventLocation, opt => opt.MapFrom(src => src.ContainsKey("cityEventLocation") ? src["cityEventLocation"].AsString() : string.Empty))
                .ForMember(dest => dest.AddressEventLocation, opt => opt.MapFrom(src => src.ContainsKey("addressEventLocation") ? src["addressEventLocation"].AsString() : string.Empty))
                .ForMember(dest => dest.InformationLocation, opt => opt.MapFrom(src => src.ContainsKey("informationEventLocation") ? src["informationEventLocation"].AsString() : string.Empty))
                .ForMember(dest => dest.GpsEventLocation, opt => opt.MapFrom(src => src.ContainsKey("gpsEventLocation") ? src["gpsEventLocation"].AsString() : string.Empty));

            CreateMap<Document, EventFirstContactProviderGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContainsKey("PK") ? src["PK"].AsString() : string.Empty))
                .ForMember(dest => dest.Screen, opt => opt.MapFrom(src => src.ContainsKey("SK") ? src["SK"].AsString() : string.Empty))
                .ForMember(dest => dest.IdProvider, opt => opt.MapFrom(src => src.ContainsKey("idProvider") ? src["idProvider"].AsString() : string.Empty))
                .ForMember(dest => dest.IdLocation, opt => opt.MapFrom(src => src.ContainsKey("idLocation") ? src["idLocation"].AsString() : string.Empty))
                .ForMember(dest => dest.CountryEventProvider, opt => opt.MapFrom(src => src.ContainsKey("countryEventProvider") ? src["countryEventProvider"].AsString() : string.Empty))
                .ForMember(dest => dest.CityEventProvider, opt => opt.MapFrom(src => src.ContainsKey("cityEventProvider") ? src["cityEventProvider"].AsString() : string.Empty))
                .ForMember(dest => dest.NearOfEventProvider, opt => opt.MapFrom(src => src.ContainsKey("nearOfEventProvider") ? src["nearOfEventProvider"].AsString() : string.Empty))
                .ForMember(dest => dest.AddressEventProvider, opt => opt.MapFrom(src => src.ContainsKey("addressEventProvider") ? src["addressEventProvider"].AsBoolean() : false))
                .ForMember(dest => dest.InformationEventProvider, opt => opt.MapFrom(src => src.ContainsKey("informationEventProvider") ? src["informationEventProvider"].AsString() : string.Empty))
                .ForMember(dest => dest.GpsEventProvider, opt => opt.MapFrom(src => src.ContainsKey("gpsEventProvider") ? src["gpsEventProvider"].AsString() : string.Empty))
                .ForMember(dest => dest.GuaranteePayment, opt => opt.MapFrom(src => src.ContainsKey("guaranteePayment") ? src["guaranteePayment"].AsDocument() : new Document()));

            CreateMap<Document, GuaranteePaymentGetDto>()
                .ForMember(dest => dest.AmountLocal, opt => opt.MapFrom(src => src.ContainsKey("amountLocal") ? src["amountLocal"].AsDouble() : 0.0))
                .ForMember(dest => dest.TypeMoney, opt => opt.MapFrom(src => src.ContainsKey("typeMoney") ? src["typeMoney"].AsString() : string.Empty))
                .ForMember(dest => dest.AmountUsd, opt => opt.MapFrom(src => src.ContainsKey("amountUsd") ? src["amountUsd"].AsDouble() : 0.0))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ContainsKey("exchangeRate") ? src["exchangeRate"].AsDouble() : 0.0))
                .ForMember(dest => dest.DeductibleAmountLocal, opt => opt.MapFrom(src => src.ContainsKey("deductibleAmountLocal") ? src["deductibleAmountLocal"].AsDouble() : 0.0))
                .ForMember(dest => dest.DeductibleAmountUsd, opt => opt.MapFrom(src => src.ContainsKey("deductibleAmountUsd") ? src["deductibleAmountUsd"].AsDouble() : 0.0));

            CreateMap<Document, InformationGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContainsKey("id") ? src["id"].AsInt() : -1))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ContainsKey("name") ? src["name"].AsString() : string.Empty));


            CreateMap<EventEmergencyContact, EventFirstContactEmergencyContactGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PartitionKey))
                .ForMember(dest => dest.Screen, opt => opt.MapFrom(src => src.ClasificationKey))
                .ForMember(dest => dest.ListEmergencyContactEvent, opt => opt.MapFrom(src => src.ListEmergencyContactEvent));

            CreateMap<GuaranteePayment, GuaranteePaymentCreatedDto>()
                .ReverseMap();

            CreateMap<GuaranteePayment, GuaranteePaymentGetDto>()
                .ReverseMap();

            CreateMap<Information, InformationDto>()
                .ReverseMap();

        }
    }
}
