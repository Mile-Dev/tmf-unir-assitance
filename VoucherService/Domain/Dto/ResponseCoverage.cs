using System.Text.Json.Serialization;

namespace VoucherService.Domain.Dto
{
    public class ResponseCoverage
    {
        [JsonPropertyName("COVERAGE_NAME")]

        public string CoverageName { get; set; } = string.Empty;

        [JsonPropertyName("COVERAGE_VALUE")]

        public string CoverageValue { get; set; } = string.Empty;
    }
}
