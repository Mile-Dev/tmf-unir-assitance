namespace EventServices.Domain.Entities
{
    public class Voucher
    {
        public int Id { get; set; }
        public int VoucherStatusId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Plan { get; set; } = string.Empty;
        public string? IssueName { get; set; } = string.Empty;
        public DateTime?  DateOfIssue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Destination { get; set; } = string.Empty;
        public bool IsCoPayment { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Llave foránea a VoucherStatus
        public Client Client { get; set; }
        public VoucherStatus VoucherStatus { get; set; }
        public ICollection<Event> Events { get; set; } 
    }

}
