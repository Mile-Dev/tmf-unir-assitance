namespace EventServices.Domain.Dto.Query
{
    public class ViewEventsGetDto
    {
        public int Id { get; set; }

        public string CodeEvent { get; set; } = string.Empty;

        public int VoucherStatusId { get; set; }

        public int EventStatusId { get; set; }

        public string Voucher { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string EventType { get; set; } = string.Empty;

        public string EventStatus { get; set; } = string.Empty;

        public string VoucherStatus { get; set; } = string.Empty;

        public string IssueBy { get; set; } = string.Empty;

        public DateTime EventStart { get; set; }

        public DateTime? EventEnd { get; set; } = null;

        public DateTime UpdatedAt { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}
