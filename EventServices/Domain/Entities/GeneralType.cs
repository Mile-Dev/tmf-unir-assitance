namespace EventServices.Domain.Entities
{
    public class GeneralType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;

        // Llave foránea a Categories
        public int CategoriesId { get; set; }
        public Category Categories { get; set; }

        // Relaciones con otras tablas
        public ICollection<ContactInformation> ContactInformation { get; set; } 
        public ICollection<Event> Events { get; set; }
        public ICollection<EventProvider> EventProviders { get; set; }

    }

}
