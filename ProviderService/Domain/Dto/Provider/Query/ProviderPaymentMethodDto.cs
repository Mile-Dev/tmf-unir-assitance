namespace ProviderService.Domain.Dto.Provider.Query
{
    public class ProviderPaymentMethodDto
    {
        public string IdPayment { get; set; } = string.Empty;

        public bool? IsActivePayment { get; set; } = null;

        public string ProviderPayment { get; set; } = string.Empty;

        public string NamePaymentMethod { get; set; } = string.Empty;

        public string TypePayment { get; set; } = string.Empty;

        public string CreatedAt { get; set; } = string.Empty;

        public string UpdatedAt { get; set; } = string.Empty;
    }
}
