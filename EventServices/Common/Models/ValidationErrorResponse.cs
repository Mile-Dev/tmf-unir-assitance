namespace EventServices.Common.Models
{
    public class ValidationErrorResponse
    {
        public string Code { get; set; } = EnumError.VALIDATION_FAILED.ToString();
        public string Message { get; set; } = "Errores de validación detectados.";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }
}
