using EventFirstContactServices.Domain.Dto.Get;

namespace EventFirstContactServices.Services
{
    public interface IEventContactEmergencyContactServices
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ideventObject"></param>
        /// <returns></returns>
        Task<EventFirstContactEmergencyContactGetDto> GetEventFirstContactEmergencyContactGetDtoByIdAsync(string ideventObject);
    }
}
