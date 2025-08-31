using System.Collections;

namespace ProviderService.Domain.Dto.ProviderLocation
{
    public class PaginatedDataQueryDto(IEnumerable data, int totalCount)
    {
        public IEnumerable Data { get; set; } = data;
        public int TotalCount { get; set; } = totalCount;
    }
}
