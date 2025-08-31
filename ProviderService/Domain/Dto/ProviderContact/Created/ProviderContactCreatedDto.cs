using ProviderService.Domain.Dto.ProviderPaymentMethod;

namespace ProviderService.Domain.Dto.ProviderContact.Created
{
    public class ProviderContactCreatedDto
    {
        public List<ListDataDto> ListData { get; set; } = [];
        public string Details { get; set; } = string.Empty;
    }
}
