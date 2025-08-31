using System.Collections;

namespace EventServices.Domain.Dto.Query
{
    public class PaginatedDataQueryDto(IEnumerable data, int totalCount)
    {
        public IEnumerable Data { get; set; } = data;
        public int TotalCount { get; set; } = totalCount;
    }
}
