namespace TrackingMokServices.Domain.Dto
{
    public class RequestMok
    {
        public string? Pk { get; set; }
      
        public string? Sk { get; set; }
        
        public RequestDataMok LogData { get; set; } = new RequestDataMok();
        
        public DateTime Timestamp { get; set; }
    }
}
