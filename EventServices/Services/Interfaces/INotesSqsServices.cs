using EventServices.Domain.Dto;

namespace EventServices.Services.Interfaces
{
    public interface INotesSqsServices
    {
        Task SendMessageAsync(RequestLogData message);
    }
}
