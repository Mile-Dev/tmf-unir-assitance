namespace PhoneConsultationService.Domain.Dto.Create
{
    public class DocumentUploadDto
    {
        public string FileName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}
