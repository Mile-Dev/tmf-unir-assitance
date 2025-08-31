namespace EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb
{
    public class ResponseEventVoucherDto
    {
        public string? Id { get; set; } = string.Empty;

        public string Screen { get; set; } = string.Empty;

        public string NameVoucher { get; set; } = string.Empty;

        public string Plan { get; set; } = string.Empty;

        public string DateOfIssue { get; set; } = string.Empty;

        public string StartDate { get; set; } = string.Empty;

        public string EndDate { get; set; } = string.Empty;

        public string IssueName { get; set; } = string.Empty;

        public InformationQueryDto IdVoucherStatus { get; set; } = new InformationQueryDto();

        public string Destination { get; set; } = string.Empty;

        public bool IsCoPayment { get; set; } = false;

        public string Description { get; set; } = string.Empty;
    }
}
