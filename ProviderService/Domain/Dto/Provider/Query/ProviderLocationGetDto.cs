
namespace ProviderService.Domain.Dto.Provider.Query
{
    public class ProviderLocationGetDto
    {

        public string IdLocation { get; set; } = string.Empty;

        public string NameProvider { get; set; } = string.Empty;

        public string TypeProvider { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Longitude { get; set; } = string.Empty;

        public string Latitude { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;
    }
}
