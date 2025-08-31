using EventFirstContactServices.Domain.Dto;
using EventFirstContactServices.Domain.Dto.Get;

namespace EventFirstContactServices.Services
{
    public interface IEventFirstContactServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventObject"></param>
        /// <returns></returns>
        Task<EventFirstContactAllGetDto> CreateUpdateEventFirstContactAsync(EventFirstContactCreateDto eventObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ideventObject"></param>
        /// <returns></returns>
        Task<EventFirstContactAllGetDto> GetEventFirstContactByIdAsync(string ideventObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ideventObject"></param>
        /// <returns></returns>
        Task<bool> DeleteEventFirstContactAsync(string ideventObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ideventObject"></param>
        /// <returns></returns>
        Task<List<EventGetDto>> GetEventAllRecordAsync(int limit);

    }
}
