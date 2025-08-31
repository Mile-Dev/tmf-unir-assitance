
namespace microservice_voucher.Domain.Dto
{
    public class SuccessResponse
    {
        public SuccessResponse(object data)
        {
            Status = "success";
            Data = data;
        }
        public string Status { get; set; }
        public object Data { get; set; }
    }
    public class ErrorResponse
    {
        public ErrorResponse(ErrorDetails error)
        {
            Status = "error";
            Error = error;
        }
        public string Status { get; set; }
        public ErrorDetails Error { get; set; }
    }

    public class ErrorDetails
    {
        public ErrorDetails(string code, string message, string details)
        {
            Code = code;
            Message = message;
            Details = details;
        }
        public string Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}