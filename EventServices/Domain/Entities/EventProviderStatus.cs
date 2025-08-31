namespace EventServices.Domain.Entities
{
    public class EventProviderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        // Relación con EventProvider
        public ICollection<EventProvider> EventProviders { get; set; } = null!;

    }

}
