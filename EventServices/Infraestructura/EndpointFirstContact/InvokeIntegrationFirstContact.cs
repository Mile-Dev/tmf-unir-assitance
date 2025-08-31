using EventServices.Common;
using EventServices.Common.HttpClient;
using EventServices.Domain.Dto;

namespace EventServices.Infraestructura.EndpointFirstContact
{
    public class InvokeIntegrationFirstContact : InvokeClientBase, IInvokeIntegrationFirstContact
    {

        public InvokeIntegrationFirstContact() : base(new InvokeClientService(new Uri(Constans.URLFirstContact)))
        {

        }

        public async Task<RequestEvent> GetFirstContactbyId(string codFirstcontact)
        {
            Console.WriteLine(" Vpc URL: " + Constans.URLFirstContact);

            var payload = new
            {
                resource = $"/api/assist/event/firstcontact/{codFirstcontact}",
                path = $"/api/assist/event/firstcontact/{codFirstcontact}",
                httpMethod = "GET",
                headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                }
            };

            ApiInvoke(Constans.URLFirstContact);
            var response = await PostAsync<object, RequestEvent>(payload);

            return response;
        }
    }
}
