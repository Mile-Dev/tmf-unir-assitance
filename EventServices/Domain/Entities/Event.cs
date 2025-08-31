using EventServices.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventServices.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public int EventStatusId { get; set; }
        public int GeneralTypesId { get; set; }
        public int VoucherId { get; set; }
        public int CustomerTripId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public string? CountryLocation { get; set; } = string.Empty;
        public string? CityLocation { get; set; } = string.Empty;
        public string? AddressLocation { get; set; } = string.Empty;
        public string? InformationLocation { get; set; } = string.Empty;
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Description { get; set; } = string.Empty;
        public int EventPriority { get; set; }
        public DateTime?  OccurredDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public EnumReasonTravel? TravelPurpose { get; set; }


        // Llaves foráneas
        [ForeignKey(nameof(EventStatusId))]
        [InverseProperty(nameof(EventStatus.Events))]
        public EventStatus? EventStatusNavigation { get; set; } 
        public string? EventStatus_Name => this.EventStatusNavigation?.Name;


        [ForeignKey(nameof(GeneralTypesId))]
        [InverseProperty(nameof(GeneralType.Events))]
        public GeneralType? GeneralTypesNavigation { get; set; }
        public string? GeneralTypes_Name => this.GeneralTypesNavigation?.Name;


        [ForeignKey(nameof(VoucherId))]
        [InverseProperty(nameof(Voucher.Events))]
        public Voucher? VoucherNavigation { get; set; } 
        public string? Voucher_Name => this.VoucherNavigation?.Name;


        [ForeignKey(nameof(CustomerTripId))]
        [InverseProperty(nameof(CustomerTrip.Events))]
        public CustomerTrip? CustomerTripNavigation { get; set; }
        public string? CustomerTrip_Name => this.CustomerTripNavigation?.Names;


        // Relaciones
        public ICollection<ContactEmergency> ?ContactEmergencies { get; set; } 
        public ICollection<EventProvider>? EventProviders { get; set; }
        public ICollection<EventNote>? Notes { get; set; }
        public ICollection<Document>? Documents { get; set; }

    }

}
