using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using EventServices.EventFirstContact.Domain.Dto;
using EventServices.EventFirstContact.Domain.Dto.Create.DynamoDb;
using EventServices.EventFirstContact.Domain.Dto.Query.DynamodDb;
using EventServices.EventFirstContact.Domain.Entities;
using EventServices.EventFirstContact.Infraestructure.DataAccesDynamodb.Interface;
using EventServices.EventFirstContact.Services.Strategy.Interfaces;

namespace EventServices.EventFirstContact.Services.Strategy
{
    public class EventEmergencyContactHandler(IEventFirstContactRepository<EventEmergencyContact> repository, IMapper mapper) : IEventFirstContactHandler
    {
        private readonly IEventFirstContactRepository<EventEmergencyContact> _repository = repository;
        private readonly IMapper _mapper = mapper;

        public string Screen => "4";

        public Task<bool> DeleteHandleAsync(string id)
        {
            try
            {
                var result = _repository.DeleteEventAsync(id, Screen);
                return Task.FromResult(result.Result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting event: {ex.Message}", ex);
            }
        }

        public async Task<object> GetByIdHandleAsync(string id)
        {
            var doc = await _repository.GetEventDraftByIdAsync(id, Screen);
            var eventResultEmergency = _mapper.Map<ResponseEventFirstContactEmergencyContactDto>(doc);
            if (doc != null)
            {
                eventResultEmergency.Id = doc.PartitionKey;
            }
            return eventResultEmergency;
        }

        public async Task HandleAsync(EventFirstContactDto eventfirstcontactdto)
        {
            if (string.IsNullOrWhiteSpace(eventfirstcontactdto.Id)) throw new Exception("Id Required for this step");
            var eventResultEmergency = _mapper.Map<EventEmergencyContact>(eventfirstcontactdto);
            eventResultEmergency.PartitionKey = eventfirstcontactdto.Id;
            eventResultEmergency.ClasificationKey = eventfirstcontactdto.Screen;
            eventResultEmergency.CreatedAt = DateTime.UtcNow.ToString("o");
            eventResultEmergency.UpdatedAt = DateTime.UtcNow.ToString("o");
            await _repository.CreateUpdateEventAsync(eventResultEmergency);

        }

        public void MapFromDocument(ResponseFirstContactDynamodb firstcontactdto, Document document)
        {
            List<EmergencyContactQueryDto>? ListEmergencyContactEvent = [];

            var listContact = document.ContainsKey("listEmergencyContact") ? document["listEmergencyContact"].AsListOfDocument() : [];

            foreach (var item in listContact)
            {
                ListEmergencyContactEvent.Add(new EmergencyContactQueryDto
                {
                    NameEmergencyContact = item.ContainsKey("nameEmergencyContact") ? item["nameEmergencyContact"].AsString() : string.Empty,
                    LastNameEmergencyContact = item.ContainsKey("lastnameEmergencyContact") ? item["lastnameEmergencyContact"].AsString() : string.Empty,
                    PhoneEmergencyContact = item.ContainsKey("PhoneEmergencyContact") ? item["PhoneEmergencyContact"].AsString() : string.Empty,
                    EmailEmergencyContact = item.ContainsKey("emailEmergencyContact") ? item["emailEmergencyContact"].AsString() : string.Empty,
                    MainPersonEmergencyContact = item.ContainsKey("mainPersonEmergencyContact") ? item["mainPersonEmergencyContact"].AsBoolean() : false,
                });
            }

            firstcontactdto.EventEmergencyContact = new ResponseEventFirstContactEmergencyContactDto
            {
                Id = document.ContainsKey("PK") ? document["PK"].AsString() : string.Empty,
                Screen = Screen,
                ListEmergencyContactEvent = ListEmergencyContactEvent
            };
        }
    }
}
