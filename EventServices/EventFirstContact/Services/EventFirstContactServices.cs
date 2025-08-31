using EventServices.EventFirstContact.Domain.Dto;
using EventServices.EventFirstContact.Domain.Dto.Create.DynamoDb;
using EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb;
using EventServices.EventFirstContact.Services.Factory.Interfaces;
using EventServices.EventFirstContact.Services.Interfaces;

namespace EventServices.EventFirstContact.Services
{
    public class EventFirstContactServices(ILogger<EventFirstContactServices> logger, IEventFirstContactHandlerFactory handlerFactory) : IEventFirstContactServices
    {
        private readonly ILogger<EventFirstContactServices> _logger = logger;

        private readonly IEventFirstContactHandlerFactory _handlerFactory = handlerFactory;

        public async Task<ResponseFirstContactDynamodb> CreateUpdateEventFirstContactAsync(EventFirstContactDto eventObject)
        {
            _logger.LogInformation("Entry method the service CreateUpdateEventFirstContactAsync");
            var handler = _handlerFactory.GetHandler(eventObject.Screen);
            await handler.HandleAsync(eventObject);

            ResponseFirstContactDynamodb responseFirstContact = new()
            {
                Id = eventObject.Id
            };

            foreach (var handlerfactory in _handlerFactory.GetAllHandlers())
            {
                var screen = handlerfactory.Screen;
                var objectoResult = await handlerfactory.GetByIdHandleAsync(eventObject.Id);
                MapObjectEventDraft(responseFirstContact, screen, objectoResult);
            }

            return responseFirstContact;
        }

        private static void MapObjectEventDraft(ResponseFirstContactDynamodb responseFirstContact, string screen, object objectoResult)
        {
            switch (screen)
            {
                case "1":
                    responseFirstContact.Event = (ResponseEventVoucherDto)objectoResult;
                    break;
                case "2":
                    responseFirstContact.EventCustomerTrip = (ResponseEventFirstContactCustomerTripDto)objectoResult;
                    break;
                case "3":
                    responseFirstContact.EventLocation = (ResponseEventFirstContactLocationDto)objectoResult;
                    break;
                case "4":
                    responseFirstContact.EventEmergencyContact = (ResponseEventFirstContactEmergencyContactDto)objectoResult;
                    break;
                case "5":
                    responseFirstContact.EventDetails = (ResponseEventFirstContactDetailsDto)objectoResult;
                    break;
                case "6":
                    responseFirstContact.EventProvider = (ResponseEventFirstContactProviderDto)objectoResult;
                    break;
                default:
                    break;
            }
        }

        public async Task<bool> DeleteEventFirstContactAsync(string ideventObject)
        {
            _logger.LogInformation("Entry method the service DeleteEventFirstContactAsync");
            var deleteTasks = new List<Task>();

            var contador = 0;
            foreach (var handlerfactory in _handlerFactory.GetAllHandlers())
            {
                var exists = await handlerfactory.GetByIdHandleAsync(ideventObject);
                if (exists != null)
                {
                    _logger.LogWarning($"Event with ID {ideventObject} does not exist.");
                    deleteTasks.Add(handlerfactory.DeleteHandleAsync(ideventObject));
                    contador++;
                }
            }

            if (contador > 0)
            {
                await Task.WhenAll(deleteTasks);
                return true;
            }

            return false;
        }

        public async Task<ResponseFirstContactDynamodb> GetEventFirstContactByIdAsync(string ideventObject)
        {
            _logger.LogInformation("Entry method the service GetEventFirstContactByIdAsync");

            ResponseFirstContactDynamodb responseFirstContact = new()
            {
                Id = ideventObject
            };

            foreach (var handlerfactory in _handlerFactory.GetAllHandlers())
            {
                var screen = handlerfactory.Screen;
                var objectoResult = await handlerfactory.GetByIdHandleAsync(ideventObject);
                MapObjectEventDraft(responseFirstContact, screen, objectoResult);
            }

            if (responseFirstContact.Event == null && responseFirstContact.EventCustomerTrip == null &&
                responseFirstContact.EventLocation == null && responseFirstContact.EventEmergencyContact == null &&
                responseFirstContact.EventDetails == null && responseFirstContact.EventProvider == null)
            {
                responseFirstContact.Id = "";
                return responseFirstContact;
            }

            return responseFirstContact;
        }

        public async Task<ResponseEventFirstContactEmergencyContactDto> GetEventFirstContactEmergenciesByIdAsync(string ideventObject)
        {
            _logger.LogInformation("Entry method the service GetEventFirstContactEmergenciesByIdAsync");
            var handler = _handlerFactory.GetHandler("4");

            var contactEmergency = await handler.GetByIdHandleAsync(ideventObject);
            ResponseEventFirstContactEmergencyContactDto EventEmergencyContact = (ResponseEventFirstContactEmergencyContactDto)contactEmergency;
            EventEmergencyContact.Screen = "4";

            return EventEmergencyContact;
        }
    }
}
