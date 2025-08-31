
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using EventServices.EventFirstContact.Infraestructure.DataAccesDynamodb.Interface;

namespace EventServices.EventFirstContact.Infraestructure.DataAccesDynamodb
{
    public class EventFirstContactRepository<T>(IDynamoDBContext context, IAmazonDynamoDB client, ILogger<EventFirstContactRepository<T>> logger) : IEventFirstContactRepository<T> where T : class
    {
        private readonly IAmazonDynamoDB _client = client;
        private readonly IDynamoDBContext _context = context;
        private readonly ILogger<EventFirstContactRepository<T>> _logger = logger;

        public async Task<bool> CreateUpdateEventAsync(T eventObject)
        {
            try
            {
                await _context.SaveAsync(eventObject);
                _logger.LogInformation("Event draft is added: {eventObject}", eventObject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist to DynamoDB Table");
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteEventAsync(string idEvent, string IdOrder)
        {
            bool result;
            try
            {
                await _context.DeleteAsync<T>(idEvent, IdOrder);
                T deletedEvent = await _context.LoadAsync<T>(idEvent, IdOrder, new DynamoDBOperationConfig
                {
                    ConsistentRead = true
                });

                result = deletedEvent == null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete event from DynamoDB Table");
                result = false;
            }
            if (result)
            {
                _logger.LogInformation("Event {Id} is deleted", idEvent);
            }
            return result;
        }

        public async Task<List<T>> GetAllDraftEventsAsync(string sortKeyValue, int limit)
        {
            var queryExpression = new QueryOperationConfig()
            {
                IndexName = "SK-createdAt-index",
                KeyExpression = new Expression
                {
                    ExpressionStatement = "SK = :skValue",
                    ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                    {
                        { ":skValue", sortKeyValue }
                    }
                },

                Limit = limit
            };

            var response = _context.FromQueryAsync<T>(queryExpression);

            var items = await response.GetRemainingAsync();
            var nextPageToken = response.PaginationToken;

            return items;
        }


        public async Task<IEnumerable<Document>> GetAllRecordEventDraftbyIdAsync(string IdEventFirstContact)
        {
            var table = Table.LoadTable(_client, "EventDraft");
            var queryConfig = new QueryOperationConfig
            {
                KeyExpression = new Expression
                {
                    ExpressionStatement = "PK = :v1",
                    ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                    {
                        { ":v1", IdEventFirstContact }
                    }
                }
            };
            var search = table.Query(queryConfig);
            var results = await search.GetNextSetAsync();
            return results;

        }


        public async Task<T> GetEventDraftByIdAsync(string IdEvent, string numberScreen)
        {
            _logger.LogInformation("Entry method the repository GetEventByIdAsync");
            return await _context.LoadAsync<T>(IdEvent, numberScreen);
        }
    }
}
