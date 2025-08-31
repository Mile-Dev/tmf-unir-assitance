namespace IssuanceMokServices.Domain.Dto
{
    public class UploadRequest
    {
        public string IssuanceName { get; set; } = default!;

        public Dictionary<string, object> Metadata { get; set; } = []; 
    }
}
