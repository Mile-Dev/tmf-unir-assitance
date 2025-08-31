namespace ProviderService.Domain.Dto.ProviderAgreement.Created
{
    public class ProviderAgreementCreatedDto
    {
        public string EndValidity { get; set; } = string.Empty;

        public string StartValidity { get; set; } = string.Empty;

        public List<string> UrlAttach { get; set; } = [];
    }
}
