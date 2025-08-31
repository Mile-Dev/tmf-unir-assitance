namespace EventServices.Domain.Dto.Create
{
    public class DocumentCreatedDto
    {
        public int EventId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string Table { get; set; } = string.Empty;

    }
}
