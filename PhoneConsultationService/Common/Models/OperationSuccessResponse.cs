namespace PhoneConsultationService.Common.Models
{
    public class OperationSuccessResponse<T>
    {
        public string Status { get; set; } = Constans.Constans.requestStatusSuccess;

        public T Data { get; set; }

        public OperationSuccessResponse(T data)
        {
            Data = data;
        }
    }
}
