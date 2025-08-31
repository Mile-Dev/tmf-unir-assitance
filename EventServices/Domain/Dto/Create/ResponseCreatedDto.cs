namespace EventServices.Domain.Dto.Create
{
    public class ResponseCreatedDto
    {
        public int Id { get; set; }

        public string CodeEvent { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
