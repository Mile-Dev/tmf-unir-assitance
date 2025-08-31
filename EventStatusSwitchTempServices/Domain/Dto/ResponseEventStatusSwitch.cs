namespace EventStatusSwitchTempServices.Domain.Dto
{
    public class ResponseEventStatusSwitch
    {
        public int Id { get; set; }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; } = null;
        public int EventStatusId { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
