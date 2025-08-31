using Amazon.DynamoDBv2.DocumentModel;

namespace EventServices.EventFirstContact.Infraestructure.DataAccesDynamodb.Interface
{
    public interface IEventFirstContactRepository<T> where T : class
    {
        /// <summary>
        /// Include new Eventv draft to the DynamoDB Table
        /// </summary>
        /// <param name="eventObject">Event to include</param>
        /// <returns>success/failure</returns>
        Task<bool> CreateUpdateEventAsync(T entity);

        /// <summary>
        /// Remove existing event from DynamoDB Table
        /// </summary>
        /// <param name="IdEvent">event to remove</param>
        /// <returns>success/failure</returns>
        Task<bool> DeleteEventAsync(string IdEvent, string IdOrder);

        /// <summary>
        /// Include new Event draft to the DynamoDB Table
        /// </summary>
        /// <param name="IdEvent">Event to include</param>
        /// <returns>success/failure</returns>
        Task<T> GetEventDraftByIdAsync(string IdEvent, string numberScreen);

        /// <summary>
        /// Get events by Id
        /// </summary>
        /// <returns>List of events</returns>
        Task<IEnumerable<Document>> GetAllRecordEventDraftbyIdAsync(string IdEventFirstContact);

        /// <summary>
        /// Get events by Id
        /// </summary>
        /// <returns>List of events</returns>
        Task<List<T>> GetAllDraftEventsAsync(string sortKeyValue, int limit);
    }
}
