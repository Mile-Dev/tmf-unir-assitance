namespace EventServices.Domain.Entities
{
    public class ContactEmergency
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Names { get; set; } = string.Empty;
        public string LastNames { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }

        // Llave foránea
        public Event Event { get; set; }
    }

}
