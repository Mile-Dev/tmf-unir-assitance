namespace EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb
{
    public class ResponseEventFirstContactLocationDto
    {
        public string Screen { get; set; } = string.Empty;

        public string? CountryEventLocation { get; set; } = null;

        public string? CityEventLocation { get; set; } = null;

        public string? AddressEventLocation { get; set; } = null;

        public string? InformationLocation { get; set; } = null;

        public string? GpsEventLocation { get; set; } = null;
    }
}
