using MasterRdsServices.Domain.Dto;

namespace MasterRdsServices.Services
{
    public interface IEventProviderStatusServices
    {
        List<EventProviderStatusQueryDto> GetEventProviderStatus();

    }
}
