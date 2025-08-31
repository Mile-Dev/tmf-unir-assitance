using ProviderService.Domain.Dto.ProviderPaymentMethod;

namespace ProviderService.Domain.Dto.ProviderContact.Query
{
    public class ProviderContactGetDto
    {
        public string IdProvider { get; set; } = string.Empty;
        public string IdContact { get; set; } = string.Empty;
        public List<ListDataDto> ListData { get; set; } = [];
        public string Details { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public string UpdatedAt { get; set; } = string.Empty;
    }
}
