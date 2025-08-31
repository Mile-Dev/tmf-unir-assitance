namespace EventServices.Domain.Entities
{
    public class EventNote
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int? IdUser { get; set; } 
        public string NameUser { get; set; } = string.Empty;
        public string RoleUser { get; set; }  = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = string.Empty;
        public string NoteType { get; set; } = string.Empty;

        // Llave foránea
        public Event Event { get; set; } 
    }

}
