namespace EventServices.Domain.Dto.Create
{
    public class EventCoveragesDto
    {
        public int EventId { get; set; }
        public string? CoverageVoucher { get; set; } = string.Empty;
    }
}
