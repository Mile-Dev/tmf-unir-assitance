using MasterRdsServices.Domain.Dto;

namespace MasterRdsServices.Services
{
    public interface IEventStatusServices
    {
        List<EventStatusQueryDto> GetEventStatusAsync();

    }
}
