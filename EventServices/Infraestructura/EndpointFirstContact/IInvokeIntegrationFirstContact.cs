using EventServices.Domain.Dto;

namespace EventServices.Infraestructura.EndpointFirstContact
{
    public interface IInvokeIntegrationFirstContact
    {
        Task<RequestEvent> GetFirstContactbyId(string codFirstcontact);

    }
}
