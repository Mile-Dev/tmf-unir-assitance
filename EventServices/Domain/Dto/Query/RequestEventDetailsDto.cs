using EventServices.Common;

namespace EventServices.Domain.Dto.Query
{
    public class RequestEventDetailsDto
    {

        //public string Screen { get; set; } = string.Empty;

        public string? DescriptionEvent { get; set; } = null;

        public int PriorityIdEvent { get; set; }

        public int TypeAssistanceIdEvent { get; set; } 

        public string? CoverageEvent { get; set; } = null;

        public DateTime? OccurredDate { get; set; } = null;

        public DateTime? ReturnDate { get; set; } = null;

        public string? TravelPurpose { get; set; } = null;

    }
}
