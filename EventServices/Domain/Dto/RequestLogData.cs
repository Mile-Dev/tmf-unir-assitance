namespace EventServices.Domain.Dto

{
    public class RequestLogData
    {
        public int EventId { get; set; }
        public string? Action { get; set; }
        public string? Description { get; set; }
        public string? EventCode { get; set; }
        public string? Role { get; set; }
        public string? SourceId { get; set; }
        public string? SourceCode { get; set; }
        public string? StatusEvent { get; set; }
        public string? StatusEventId { get; set; }
        public string? UserName { get; set; }
        public bool SendToClient { get; set; } = false;
    }
}
