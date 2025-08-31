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
    public class EventLocationHandler(IEventFirstContactRepository<EventLocation> repository, IMapper mapper) : IEventFirstContactHandler
    {
        private readonly IEventFirstContactRepository<EventLocation> _repository = repository;
        private readonly IMapper _mapper = mapper;

        public string Screen => "3";

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
            var resultEventFirstContactLocationGetDto = _mapper.Map<ResponseEventFirstContactLocationDto>(doc);
            if (doc != null)
            {
                resultEventFirstContactLocationGetDto.Screen = doc.ClasificationKey;
            }
            return resultEventFirstContactLocationGetDto;
        }

        public async Task HandleAsync(EventFirstContactDto eventfirstcontactdto)
        {
            if (string.IsNullOrWhiteSpace(eventfirstcontactdto.Id)) throw new Exception("Id Required for this step");
            var eventResultLocation = _mapper.Map<EventLocation>(eventfirstcontactdto);
            eventResultLocation.PartitionKey = eventfirstcontactdto.Id;
            eventResultLocation.ClasificationKey = eventfirstcontactdto.Screen;
            eventResultLocation.CreatedAt = DateTime.UtcNow.ToString("o");
            eventResultLocation.UpdatedAt = DateTime.UtcNow.ToString("o");
            await _repository.CreateUpdateEventAsync(eventResultLocation);

        }

        public void MapFromDocument(ResponseFirstContactDynamodb firstcontactdto, Document document)
        {
            firstcontactdto.EventLocation = _mapper.Map<ResponseEventFirstContactLocationDto>(document);
        }
    }
}
