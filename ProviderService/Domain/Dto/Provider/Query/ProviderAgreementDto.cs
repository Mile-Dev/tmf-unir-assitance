
namespace ProviderService.Domain.Dto.Provider.Query
{
    public class ProviderAgreementDto
    {
        public string IdAgreement { get; set; } = string.Empty;

        public string EndValidity { get; set; } = string.Empty;

        public string StartValidity { get; set; } = string.Empty;

        public List<string> UrlAttach { get; set; } = [];

        public string CreatedAt { get; set; } = string.Empty;
    }
}
