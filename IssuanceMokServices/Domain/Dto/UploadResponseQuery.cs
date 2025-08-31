
namespace IssuanceMokServices.Domain.Dto
{
    public class UploadResponseQuery
    {
        public string Id { get; set; } = default!;

        public string IssuanceName { get; set; } = string.Empty;

        public string UrlDownload { get; set; } = string.Empty;

        public Dictionary<string, object> Metadata { get; set; } = [];

        public string CreatedAt { get; set; } = string.Empty;

        public string UpdatedAt { get; set; } = string.Empty;
    }
}
