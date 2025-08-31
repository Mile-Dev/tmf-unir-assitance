namespace ProviderService.Domain.Dto.Provider.Query
{
    public class ProviderGetDto
    {
        public required string Id { get; set; }
        public required string NameProvider { get; set; }
        public string? TypeProvider { get; set; }
        public int? Score { get; set; }
        public string? Nit { get; set; }
        public string? IdFiscal { get; set; }
        public string? Details { get; set; }
        public List<ProviderLocationGetDto> Locations { get; set; } = [];
        public List<ProviderContactDto> Contacts { get; set; } = [];
        public List<ProviderAgreementDto> Agreements { get; set; } = [];
        public List<ProviderPaymentMethodDto> PaymentMethod { get; set; } = [];
    }
}
