namespace EventStatusSwitchTempServices.Domain.Entities
{
    public class EventStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Relación con Events
        public ICollection<Events> Events { get; set; }
    }
}
