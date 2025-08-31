namespace EventServices.Domain.Entities
{
    public class ContactInformation
    {
        public int Id { get; set; }
        public int CustomerTripId { get; set; }
        public int GeneralTypesId { get; set; }
        public string Value { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Llaves foráneas
        public CustomerTrip CustomerTrip { get; set; }
        public GeneralType GeneralTypes { get; set; }
    }

}
