using Amazon.DynamoDBv2.DocumentModel;
using EventServices.EventFirstContact.Domain.Dto;
using EventServices.EventFirstContact.Domain.Dto.Create.DynamoDb;

namespace EventServices.EventFirstContact.Services.Strategy.Interfaces
{
    public interface IEventFirstContactHandler
    {
        string Screen { get; }

        Task HandleAsync(EventFirstContactDto eventfirstcontactdto);

        Task<bool> DeleteHandleAsync(string id);

        void MapFromDocument(ResponseFirstContactDynamodb firstcontactdto, Document document);

        Task<object> GetByIdHandleAsync(string id);


    }
}
