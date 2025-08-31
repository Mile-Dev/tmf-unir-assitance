using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using EventFirstContactServices.Domain.Dto;
using EventFirstContactServices.Domain.Dto.Get;
using EventFirstContactServices.Domain.Entities;
using EventFirstContactServices.Domain.Interfaces;

namespace EventFirstContactServices.Services
{
    public class EventFirstContactServices(IEventFirstContactRepository<Event> repository, IEventFirstContactRepository<EventCustomerTrip> repositorytrip, IEventFirstContactRepository<EventDetails> repositoryEventDetails,
               IEventFirstContactRepository<EventEmergencyContact> repositoryEventEmergencyContact, IEventFirstContactRepository<EventLocation> repositoryEventLocation,
               IEventFirstContactRepository<EventProvider> repositoryEventEventProvider, IMapper mapper, ILogger<EventFirstContactServices> logger) : IEventFirstContactServices
    {
        private readonly IEventFirstContactRepository<Event> _repository = repository;
        private readonly IEventFirstContactRepository<EventCustomerTrip> _repositoryEventCustomerTrip = repositorytrip;
        private readonly IEventFirstContactRepository<EventLocation> _repositoryEventLocation = repositoryEventLocation;
        private readonly IEventFirstContactRepository<EventEmergencyContact> _repositoryEventEmergencyContact = repositoryEventEmergencyContact;
        private readonly IEventFirstContactRepository<EventDetails> _repositoryEventDetails = repositoryEventDetails;
        private readonly IEventFirstContactRepository<EventProvider> _repositoryEventEventProvider = repositoryEventEventProvider;

        private readonly ILogger<EventFirstContactServices> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<EventFirstContactAllGetDto> CreateUpdateEventFirstContactAsync(EventFirstContactCreateDto eventObject)
        {
            _logger.LogInformation("Entry method the service CreateUpdateEventFirstContactAsync");
            var existId = string.IsNullOrWhiteSpace(eventObject.Id);

            switch (eventObject.Screen)
            {
                case "1":
                    var eventResult = _mapper.Map<Event>(eventObject);
                    eventResult.PartitionKey = string.IsNullOrWhiteSpace(eventObject.Id) ? Guid.NewGuid().ToString() : eventObject.Id;
                    eventResult.ClasificationKey = eventObject.Screen;
                    eventResult.CreatedAt = DateTime.Now.ToString("o");
                    eventResult.UpdatedAt = DateTime.Now.ToString("o");
                    await _repository.CreateUpdateEventAsync(eventResult);
                    eventObject.Id = eventResult.PartitionKey;
                    break;
                case "2":
                    if (existId) { throw new Exception("Id Required id for this step"); }
                    var eventResultTrip = _mapper.Map<EventCustomerTrip>(eventObject);
                    eventResultTrip.PartitionKey = eventObject.Id;
                    eventResultTrip.ClasificationKey = eventObject.Screen;
                    eventResultTrip.CreatedAt = DateTime.Now.ToString("o");
                    eventResultTrip.UpdatedAt = DateTime.Now.ToString("o");
                    await _repositoryEventCustomerTrip.CreateUpdateEventAsync(eventResultTrip);

                    break;
                case "3":
                    if (existId) { throw new Exception("Id Required for this step"); }
                    var eventResultLocation = _mapper.Map<EventLocation>(eventObject);
                    eventResultLocation.PartitionKey = eventObject.Id;
                    eventResultLocation.ClasificationKey = eventObject.Screen;
                    eventResultLocation.CreatedAt = DateTime.UtcNow.ToString("o");
                    eventResultLocation.UpdatedAt = DateTime.UtcNow.ToString("o");
                    await _repositoryEventLocation.CreateUpdateEventAsync(eventResultLocation);
                    break;
                case "4":
                    if (existId) { throw new Exception("Id Required id for this step"); }
                    var eventResultEmergency = _mapper.Map<EventEmergencyContact>(eventObject);
                    eventResultEmergency.PartitionKey = eventObject.Id;
                    eventResultEmergency.ClasificationKey = eventObject.Screen;
                    eventResultEmergency.CreatedAt = DateTime.UtcNow.ToString("o");
                    eventResultEmergency.UpdatedAt = DateTime.UtcNow.ToString("o");
                    await _repositoryEventEmergencyContact.CreateUpdateEventAsync(eventResultEmergency);
                    break;
                case "5":
                    if (existId) { throw new Exception("Id Required id for this step"); }
                    var eventResultDetails = _mapper.Map<EventDetails>(eventObject);
                    eventResultDetails.PartitionKey = eventObject.Id;
                    eventResultDetails.ClasificationKey = eventObject.Screen;
                    eventResultDetails.CreatedAt = DateTime.UtcNow.ToString("o");
                    eventResultDetails.UpdatedAt = DateTime.UtcNow.ToString("o");
                    await _repositoryEventDetails.CreateUpdateEventAsync(eventResultDetails);
                    break;
                case "6":
                    if (existId) { throw new Exception("Id Required id for this step"); }
                    var eventResultProvider = _mapper.Map<EventProvider>(eventObject);
                    eventResultProvider.PartitionKey = eventObject.Id;
                    eventResultProvider.ClasificationKey = eventObject.Screen;
                    eventResultProvider.CreatedAt = DateTime.UtcNow.ToString("o");
                    eventResultProvider.UpdatedAt = DateTime.UtcNow.ToString("o");
                    await _repositoryEventEventProvider.CreateUpdateEventAsync(eventResultProvider);
                    break;
                default:
                    throw new Exception("Invalid screen");
            }

            EventFirstContactAllGetDto eventResultAll = await GetObjectFirstContact(eventObject.Id);

            return eventResultAll;
        }

        private async Task<EventFirstContactAllGetDto> GetObjectFirstContact(string idEventDraft)
        {
            var Documents = await _repository.GetAllRecordEventDraftbyIdAsync(idEventDraft);

            var eventResultAll = new EventFirstContactAllGetDto
            {
                Id = idEventDraft
            };

            foreach (var Document in Documents)
            {
                var screen = Document.ContainsKey("SK") ? Document["SK"].AsString() : string.Empty;
                MapObjectEventDraft(eventResultAll, Document, screen);
            }

            return eventResultAll;
        }

        private void MapObjectEventDraft(EventFirstContactAllGetDto eventResultAll, Document Document, string screen)
        {
            switch (screen)
            {
                case "1":
                    eventResultAll.Event = _mapper.Map<EventGetDto>(Document);
                    break;
                case "2":
                    eventResultAll.EventCustomerTrip = _mapper.Map<EventFirstContactCustomerTripGetDto>(Document);
                    break;
                case "3":
                    eventResultAll.EventLocation = _mapper.Map<EventFirstContactLocationGetDto>(Document);
                    break;
                case "4":
                    MapObjectEmergencyContact(eventResultAll, Document, screen);
                    break;
                case "5":
                    eventResultAll.EventDetails = _mapper.Map<EventFirstContactDetailsGetDto>(Document);
                    break;
                case "6":
                    eventResultAll.EventProvider = _mapper.Map<EventFirstContactProviderGetDto>(Document);
                    break;
                default:
                    break;
            }
        }

        private static void MapObjectEmergencyContact(EventFirstContactAllGetDto eventResultAll, Document Document, string screen)
        {
            List<EmergencyContactDto>? ListEmergencyContactEvent = [];
            var listContact = Document.ContainsKey("listEmergencyContact") ? Document["listEmergencyContact"].AsListOfDocument() : [];
            foreach (var item in listContact)
            {
                ListEmergencyContactEvent.Add(new EmergencyContactDto
                {
                    NameEmergencyContact = item.ContainsKey("nameEmergencyContact") ? item["nameEmergencyContact"].AsString() : string.Empty,
                    LastNameEmergencyContact = item.ContainsKey("lastnameEmergencyContact") ? item["lastnameEmergencyContact"].AsString() : string.Empty,
                    PhoneEmergencyContact = item.ContainsKey("PhoneEmergencyContact") ? item["PhoneEmergencyContact"].AsString() : string.Empty,
                    EmailEmergencyContact = item.ContainsKey("emailEmergencyContact") ? item["emailEmergencyContact"].AsString() : string.Empty,
                    MainPersonEmergencyContact = item.ContainsKey("mainPersonEmergencyContact") ? item["mainPersonEmergencyContact"].AsBoolean() : false,
                });
            }

            eventResultAll.EventEmergencyContact = new EventFirstContactEmergencyContactGetDto
            {
                Id = Document.ContainsKey("PK") ? Document["PK"].AsString() : string.Empty,
                Screen = screen,
                ListEmergencyContactEvent = ListEmergencyContactEvent
            };
        }

        public async Task<bool> DeleteEventFirstContactAsync(string ideventObject)
        {
            _logger.LogInformation("Entry method the service DeleteEventFirstContactAsync");
            try
            {
                var deleteTasks = new List<Task>
            {
                _repository.DeleteEventAsync(ideventObject, "1"),
                _repositoryEventCustomerTrip.DeleteEventAsync(ideventObject, "2"),
                _repositoryEventLocation.DeleteEventAsync(ideventObject, "3"),
                _repositoryEventEmergencyContact.DeleteEventAsync(ideventObject, "4"),
                _repositoryEventDetails.DeleteEventAsync(ideventObject, "5"),
                _repositoryEventEventProvider.DeleteEventAsync(ideventObject, "6")
            };

                await Task.WhenAll(deleteTasks);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete event from DynamoDB Table");
                throw new Exception("Failed to delete event from DynamoDB Table");
            }
        }

        public async Task<EventFirstContactAllGetDto> GetEventFirstContactByIdAsync(string ideventObject)
        {
            _logger.LogInformation("Entry method the service GetEventFirstContactByIdAsync");
            EventFirstContactAllGetDto eventResultAll = await GetObjectFirstContact(ideventObject);
            return eventResultAll;
        }

        public async Task<List<EventGetDto>> GetEventAllRecordAsync(int limit = 10)
        {
            _logger.LogInformation("Entry method the service GetEventAllRecordAsync");
            var result  = await _repository.GetAllDraftEventsAsync("1", limit);
            var listEvents = _mapper.Map<List<EventGetDto>>(result);

            return listEvents;
        }
    }
}
