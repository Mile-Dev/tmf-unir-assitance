using EventStatusSwitchTempServices.Domain.Dto;

namespace EventStatusSwitchTempServices.Services
{
    public interface IEventStatusSwitchServices
    {
        Task<ResponseEventStatusSwitch> UpdateStatusEventById(RequestEventStatus eventSwitch);
    }
}
