namespace EventServices.Domain.Dto.Query
{
    public class RequestEventLocationDto
    {
        public string? CountryEventLocation { get; set; } = null;

        public string? CityEventLocation { get; set; } = null;

        public string? AddressEventLocation { get; set; } = null;

        public string? InformationLocation { get; set; } = null;

        public string? GpsEventLocation { get; set; } = null;
    }
}
