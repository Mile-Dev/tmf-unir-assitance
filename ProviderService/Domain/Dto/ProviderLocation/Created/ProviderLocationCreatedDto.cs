namespace ProviderService.Domain.Dto.ProviderLocation.Created
{
    public class ProviderLocationCreatedDto
    {
        public string IdCountry { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string IdCity { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
