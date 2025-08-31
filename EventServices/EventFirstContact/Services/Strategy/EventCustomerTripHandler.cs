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
    public class EventCustomerTripHandler(IEventFirstContactRepository<EventCustomerTrip> repository, IMapper mapper) : IEventFirstContactHandler
    {
        private readonly IEventFirstContactRepository<EventCustomerTrip> _repository = repository;
        private readonly IMapper _mapper = mapper;

        public string Screen => "2";

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
            var resultEventFirstContactCustomerTripGetDto = _mapper.Map<ResponseEventFirstContactCustomerTripDto>(doc);
            if (doc != null)
            {
                resultEventFirstContactCustomerTripGetDto.Screen = doc.ClasificationKey;
            }
            return resultEventFirstContactCustomerTripGetDto;
        }

        public async Task HandleAsync(EventFirstContactDto eventfirstcontactdto)
        {
            if (string.IsNullOrWhiteSpace(eventfirstcontactdto.Id)) throw new Exception("Id Required for this step");

            var entity = _mapper.Map<EventCustomerTrip>(eventfirstcontactdto);
            entity.PartitionKey = eventfirstcontactdto.Id;
            entity.ClasificationKey = eventfirstcontactdto.Screen;
            entity.CreatedAt = DateTime.UtcNow.ToString("o");
            entity.UpdatedAt = DateTime.UtcNow.ToString("o");
            await _repository.CreateUpdateEventAsync(entity);
        }

        public void MapFromDocument(ResponseFirstContactDynamodb firstcontactdto, Document document)
        {
            firstcontactdto.EventCustomerTrip = _mapper.Map<ResponseEventFirstContactCustomerTripDto>(document);
        }
    }
}
