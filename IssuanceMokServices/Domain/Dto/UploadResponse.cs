
namespace IssuanceMokServices.Domain.Dto
{
    public class UploadResponse
    {
        public string Id { get; init; } = default!;

        public string IssuanceName { get; set; } = string.Empty;

        public string UrlDocument { get; set; } = string.Empty;

        public string CreatedAt { get; set; } = string.Empty;

        public string UpdatedAt { get; set; } = string.Empty;
    }
}
