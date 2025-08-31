namespace ProviderService.Domain.Dto.Provider
{
    public class RequestQueryDto
    {
        public string? IdCountry { get; set; } = null;
        public string? IdCity { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? Type { get; set; } = null;
        public string? Geohash { get; set; } = null;
        public string? Latitude { get; set; } = null;
        public string? Longitude { get; set; } = null;
        public int? RadioKMS { get; set; } = 0;
        public bool TypeExists { get; set; } = false;
        public bool NameExists { get; set; } = false;
        public int Limit { get; set; } = 10;
        public string? PaginationToken { get; set; }
    }
}
