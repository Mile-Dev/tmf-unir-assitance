using ProviderService.Domain.Entities;

namespace ProviderService.Domain.Dto.Provider.Query
{
    public class ProviderContactDto
    {
       public string IdContact { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;

        public List<ListData> ListData { get; set; } = [];

        public string CreatedAt { get; set; } = string.Empty;

        public string UpdatedAt { get; set; } = string.Empty;
    }
}
