namespace EventServices.Common.Models
{
    public class OperationErrorsResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public object? Details { get; set; }

        public OperationErrorsResponse(string code, string message, object? details)
        {
            Code = code;
            Message = message;
            Details = details;
        }
    }
}
