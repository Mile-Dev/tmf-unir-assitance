using System.ComponentModel.DataAnnotations;

namespace ProviderService.Domain.Dto.Provider.Created
{
    public class ProviderCreateDto
    {
        [Required(ErrorMessage = "The field NameProvider it is required")]
        public required string NameProvider { get; set; }
        public string? TypeProvider { get; set; }
        public int Score { get; set; }
        public string? Nit { get; set; }
        public string? IdFiscal { get; set; }
        public string? Details { get; set; }
    }
}
