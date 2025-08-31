using System.ComponentModel.DataAnnotations.Schema;

namespace EventServices.Domain.Entities
{
    public class Document
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string Table { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [ForeignKey(nameof(EventId))]
        [InverseProperty(nameof(Event.Documents))]
        public Event? EventNavigation { get; set; }
    }
}
