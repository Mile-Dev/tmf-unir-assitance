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
    public class EventHandler(IEventFirstContactRepository<Event> repository, IMapper mapper) : IEventFirstContactHandler
    {
        private readonly IEventFirstContactRepository<Event> _repository = repository;
        private readonly IMapper _mapper = mapper;

        public string Screen => "1";

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
            var eventGetDto = _mapper.Map<ResponseEventVoucherDto>(doc);
            if (doc != null)
            {
                eventGetDto.Id = doc.PartitionKey;
                eventGetDto.Screen = doc.ClasificationKey;
            }
            return eventGetDto;
        }

        public async Task HandleAsync(EventFirstContactDto eventfirstcontactdto)
        {
            var entity = _mapper.Map<Event>(eventfirstcontactdto);
            entity.PartitionKey = string.IsNullOrWhiteSpace(eventfirstcontactdto.Id) ? Guid.NewGuid().ToString() : eventfirstcontactdto.Id;
            entity.ClasificationKey = eventfirstcontactdto.Screen;
            entity.CreatedAt = DateTime.UtcNow.ToString("o");
            entity.UpdatedAt = DateTime.UtcNow.ToString("o");
            await _repository.CreateUpdateEventAsync(entity);
            eventfirstcontactdto.Id = entity.PartitionKey;
        }

        public void MapFromDocument(ResponseFirstContactDynamodb firstcontactdto, Document document)
        {
            firstcontactdto.Event = _mapper.Map<ResponseEventVoucherDto>(document);
        }
    }
}
