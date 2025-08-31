namespace EventServices.Domain.Entities
{
    public class ViewEventDetail
    {
        public int IdClient { get; set; }

        public string CodeClient { get; set; } = string.Empty;

        public int Id { get; set; }

        public string CodeEvent { get; set; } = string.Empty;

        public string? Description { get; set; } = null;

        public string Voucher { get; set; } = string.Empty;

        public string PlanVoucher { get; set; } = string.Empty;

        public string IssueBy { get; set; } = string.Empty;

        public DateTime DateOfIssueVoucher { get; set; }

        public DateTime StartDateVoucher { get; set; }

        public DateTime EndDateVoucher { get; set; }

        public int MissingDays { get; set; }

        public string StatusVoucher { get; set; } = string.Empty;

        public string DestinationVoucher { get; set; } = string.Empty;

        public string TypeIdentification { get; set; } = string.Empty;
       
        public string Identification { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Names { get; set; } = string.Empty;

        public string LastNames { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string CountryOfBirth { get; set; } = string.Empty;

        public string EventType { get; set; } = string.Empty;

        public int EventStatusId { get; set; } 

        public string EventStatus { get; set; } = string.Empty;

        public string? AssignedTo { get; set; } = null;

        public DateTime EventStart { get; set; }

        public DateTime? EventEnd { get; set; } = null;

        public decimal? ActualAmountUsd { get; set; }

        public string? Country { get; set; } = string.Empty;
        
        public string? City { get; set; } = string.Empty;
        
        public string? Address { get; set; } = string.Empty;
    }
}
