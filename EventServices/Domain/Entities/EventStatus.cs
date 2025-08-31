namespace EventServices.Domain.Entities
{
    public class EventStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        // Relación con Events
        public ICollection<Event> Events { get; set; } 
    }

}
