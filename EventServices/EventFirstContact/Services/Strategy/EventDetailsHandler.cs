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
    public class EventDetailsHandler(IEventFirstContactRepository<EventDetails> repository, IMapper mapper) : IEventFirstContactHandler
    {
        private readonly IEventFirstContactRepository<EventDetails> _repository = repository;
        private readonly IMapper _mapper = mapper;

        public string Screen => "5";

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
            var resultEventFirstContactDetailsGetDto = _mapper.Map<ResponseEventFirstContactDetailsDto>(doc);
            if (doc != null)
            {
                resultEventFirstContactDetailsGetDto.Screen = doc.ClasificationKey;
            }
            return resultEventFirstContactDetailsGetDto;
        }

        public async Task HandleAsync(EventFirstContactDto eventfirstcontactdto)
        {
            if (string.IsNullOrWhiteSpace(eventfirstcontactdto.Id)) throw new Exception("Id Required for this step");
            var eventResultDetails = _mapper.Map<EventDetails>(eventfirstcontactdto);
            eventResultDetails.PartitionKey = eventfirstcontactdto.Id;
            eventResultDetails.ClasificationKey = eventfirstcontactdto.Screen;
            eventResultDetails.CreatedAt = DateTime.UtcNow.ToString("o");
            eventResultDetails.UpdatedAt = DateTime.UtcNow.ToString("o");
            await _repository.CreateUpdateEventAsync(eventResultDetails);
        }

        public void MapFromDocument(ResponseFirstContactDynamodb firstcontactdto, Document document)
        {
            firstcontactdto.EventDetails = _mapper.Map<ResponseEventFirstContactDetailsDto>(document);
        }
    }
}
