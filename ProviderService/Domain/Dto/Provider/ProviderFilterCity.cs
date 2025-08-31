namespace ProviderService.Domain.Dto.Provider
{
    public class ProviderFilterCity 
    {
        public string? IdCountry { get; set; }
        public string? IdCity { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public int? RadioKMS { get; set; }
        public int? Limit { get; set; }
    }
}
