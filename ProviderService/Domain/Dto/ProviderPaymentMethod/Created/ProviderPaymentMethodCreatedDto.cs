namespace ProviderService.Domain.Dto.ProviderPaymentMethod.Created
{
    public class ProviderPaymentMethodCreatedDto
    {
        public string TypePayment { get; set; } = string.Empty;

        public List<ListDataDto> ListData { get; set; } = [];

        public string Details { get; set; } = string.Empty;
    }
}
