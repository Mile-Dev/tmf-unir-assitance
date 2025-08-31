namespace ProviderService.Domain.Dto.ProviderPaymentMethod.Query
{
    public class ProviderPaymentMethodGetDto
    {
        public string IdProvider { get; init; } = string.Empty;

        public string IdPayment { get; set; } = string.Empty;

        public bool? IsActivePayment { get; set; } = null;

        public string TypePayment { get; set; } = string.Empty;

        public List<ListDataDto> ListData { get; set; } = [];

        public string CreatedAt { get; set; } = string.Empty;

        public string UpdatedAt { get; set; } = string.Empty;
    }
}
