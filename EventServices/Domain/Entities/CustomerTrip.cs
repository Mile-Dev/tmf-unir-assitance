namespace EventServices.Domain.Entities
{
    public class CustomerTrip
    {
        public int Id { get; set; }
        public string? IdClinicHistory { get; set; }
        public string Names { get; set; } = string.Empty;
        public string LastNames { get; set; } = string.Empty;
        public string CountryOfBirth { get; set; } = string.Empty;
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relaciones con Vouchers, Events y ContactInformation
        public ICollection<Event> Events { get; set; } 
        public ICollection<ContactInformation> ContactInformation { get; set; } 
    }

}
