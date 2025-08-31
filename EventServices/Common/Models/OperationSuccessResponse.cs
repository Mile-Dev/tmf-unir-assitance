namespace EventServices.Common.Models
{
    public class OperationSuccessResponse<T>
    {
        public string Status { get; set; } = Constans.RequestStatusSuccess;
       
        public T Data { get; set; }

        public OperationSuccessResponse(T data)
        {
            Data = data;
        }
    }
}
