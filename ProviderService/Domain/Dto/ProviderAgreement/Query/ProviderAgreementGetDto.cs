namespace ProviderService.Domain.Dto.ProviderAgreement.Query
{
    public class ProviderAgreementGetDto
    {
        public string IdProvider { get; set; } = string.Empty;

        public string IdAgreement { get; set; } = string.Empty;

        public string EndValidity { get; set; } = string.Empty;

        public string StartValidity { get; set; } = string.Empty;

        public List<string> UrlAttach { get; set; } = [];

        public string UpdatedAt { get; set; } = string.Empty;

        public string CreatedAt { get; set; } = string.Empty;
    }
}
