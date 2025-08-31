namespace EventServices.Domain.Dto
{
    public class ResponseEventStatus
    {
        public int Id { get; set; }
        public string EventStatus { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
