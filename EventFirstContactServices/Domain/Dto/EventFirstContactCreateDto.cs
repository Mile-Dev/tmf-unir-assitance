using EventFirstContactServices.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EventFirstContactServices.Domain.Dto
{
    public class EventFirstContactCreateDto
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "The field Screen it is required")]
        public string Screen { get; set; } = string.Empty;

        public string? NameVoucher { get; set; } = null;

        public string? Plan { get; set; } = null;

        public string? DateOfIssue { get; set; } = null;

        public string? StartDate { get; set; } = null;

        public string? EndDate { get; set; } = null;

        public string? IssueName { get; set; } = null;

        public InformationDto IdVoucherStatus { get; set; } = new InformationDto();

        public string? Destination { get; set; } = null;

        public bool IsCoPayment { get; set; } = false;

        public string? Description { get; set; } = null;

        //Field of CustomerTrip number step 2

        public string? NameCustomerTrip { get; set; } = null;

        public string? LastNameCustomerTrip { get; set; } = null;

        public string? EmailCustomerTrip { get; set; } = null;

        public string? CellPhoneCustomerTrip { get; set; } = null;

        public InformationDto TypeIdentificationPhoneCustomerTrip { get; set; } = new InformationDto();

        public string? IdentificationPhoneCustomerTrip { get; set; } = null;
      
        public string? CountryOfBirthCustomerTrip { get; set; } = null;

        public int GenderCustomerTrip { get; set; }

        public string? BirthDateCustomerTrip { get; set; } = null;

        //Field of Location number step 3

        public string? CountryEventLocation { get; set; } = null;

        public string? CityEventLocation { get; set; } = null;

        public string? AddressEventLocation { get; set; } = null;

        public string? GpsEventLocation { get; set; } = null;

        public string? InformationLocation { get; set; } = null;

        //Field of EmergencyContact number step 4

        public List<EmergencyContactDto>? ListEmergencyContactEvent { get; set; } = null;

        //Field of Event Details number step 5

        public string? DescriptionEventDetails { get; set; } = null;

        public InformationDto PriorityEventDetails { get; set; } = new InformationDto();    

        public InformationDto CategorieEventDetails { get; set; } = new InformationDto();

        public InformationDto TypeEventDetails { get; set; } = new InformationDto();

        public string? CoverageEventDetails { get; set; } = null;

        public string? CoverageDetailVoucherEventDetails { get; set; } = null;

        public bool? RequireProviderEventDetails { get; set; } = null;

        public string? SelectedProviderEventDetails { get; set; } = null;

        public bool? RequirePhoneMedicalConsultationEventDetails { get; set; } = null;

        public bool? RequireReviewAssistsTeamEventDetails { get; set; } = null;

        public string? SelectedReviewAssistsTeamEventDetails { get; set; } = null;

        public bool? RequireReviewCashBackTeamEventDetails { get; set; } = null;

        //Field of Event Provider number step 6

        public string IdProvider { get; set; } = string.Empty;

        public string IdLocation { get; set; } = string.Empty;

        public string? CountryEventProvider { get; set; } = null;

        public string? CityEventProvider { get; set; } = null;

        public string? NearOfEventProvider { get; set; } = null;

        public string? AddressEventProvider { get; set; } = null;

        public string? InformationEventProvider { get; set; } = null;

        public string? GpsEventProvider { get; set; } = null;

        public string? ScheduledAppointment { get; set; } = null;

        public GuaranteePaymentCreatedDto? GuaranteePayment { get; set; }

    }

    public class GuaranteePaymentCreatedDto
    {
        public decimal? AmountLocal { get; set; } = null;
        public string? TypeMoney { get; set; } = null;
        public decimal? AmountUsd { get; set; } = null;
        public decimal? ExchangeRate { get; set; } = null;
        public decimal? DeductibleAmountLocal { get; set; } = null;
        public decimal? DeductibleAmountUsd { get; set; } = null;
    }

    public class InformationDto
    {
        public int Id{ get; set; }
        public string Name { get; set; }  = string.Empty;
    }
}
