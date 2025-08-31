namespace EventServices.Domain.Dto.Create
{
    public class ResponseUpdatedDto
    {
        public int Id { get; set; }
        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }
}
