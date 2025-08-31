using EventServices.EventFirstContact.Domain.Dto;
using EventServices.EventFirstContact.Domain.Dto.Create.DynamoDb;
using EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb;

namespace EventServices.EventFirstContact.Services.Interfaces
{
    public interface IEventFirstContactServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventObject"></param>
        /// <returns></returns>
        Task<ResponseFirstContactDynamodb> CreateUpdateEventFirstContactAsync(EventFirstContactDto eventObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ideventObject"></param>
        /// <returns></returns>
        Task<ResponseFirstContactDynamodb> GetEventFirstContactByIdAsync(string ideventObject);

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
        Task<ResponseEventFirstContactEmergencyContactDto> GetEventFirstContactEmergenciesByIdAsync(string ideventObject);
    }
}
