namespace EventServices.Domain.Dto.Query
{
    public class RequestEventVoucherDto
    {
        public string NameVoucher { get; set; } = string.Empty;

        public string? Plan { get; set; } = string.Empty;

        public string? DateOfIssue { get; set; } = string.Empty;

        public string? StartDate { get; set; } = string.Empty;

        public string? EndDate { get; set; } = string.Empty;

        public string? IssueName { get; set; } = string.Empty;
     
        public int VoucherStatusId { get; set; }
        
        public string? Destination { get; set; } = string.Empty;

        public bool IsCoPayment { get; set; } = false;

        public string? Description { get; set; } = string.Empty;
    }
}
