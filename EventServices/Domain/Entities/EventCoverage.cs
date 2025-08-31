namespace EventServices.Domain.Entities
{
    public class EventCoverage
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string CoverageVoucher { get; set; } = string.Empty;
        public string CoverageVoucherDetails { get; set; } = string.Empty;

        // Llave foránea
        public Event Event { get; set; } = null!;
    }
}
