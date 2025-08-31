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
    public class EventProviderHandler(IEventFirstContactRepository<EventProvider> repository, IMapper mapper) : IEventFirstContactHandler
    {
        private readonly IEventFirstContactRepository<EventProvider> _repository = repository;
        private readonly IMapper _mapper = mapper;

        public string Screen => "6";

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
            var resultEventFirstContactProviderGetDto = _mapper.Map<ResponseEventFirstContactProviderDto>(doc);
            if (doc != null)
            {
                resultEventFirstContactProviderGetDto.Screen = doc.ClasificationKey;
            }
            return resultEventFirstContactProviderGetDto;
        }

        public async Task HandleAsync(EventFirstContactDto eventfirstcontactdto)
        {
            if (string.IsNullOrWhiteSpace(eventfirstcontactdto.Id)) throw new Exception("Id Required for this step");
            var eventResultProvider = _mapper.Map<EventProvider>(eventfirstcontactdto);
            eventResultProvider.PartitionKey = eventfirstcontactdto.Id;
            eventResultProvider.ClasificationKey = eventfirstcontactdto.Screen;
            eventResultProvider.CreatedAt = DateTime.UtcNow.ToString("o");
            eventResultProvider.UpdatedAt = DateTime.UtcNow.ToString("o");
            await _repository.CreateUpdateEventAsync(eventResultProvider);
        }

        public void MapFromDocument(ResponseFirstContactDynamodb firstcontactdto, Document document)
        {
            firstcontactdto.EventProvider = _mapper.Map<ResponseEventFirstContactProviderDto>(document);
        }
    }
}
