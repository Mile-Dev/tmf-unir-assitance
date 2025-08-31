using System.Text.Json;
using System.Text;
using System.Web;
using EventServices.Common.HttpClient.Interface;

namespace EventServices.Common.HttpClient
{
    public class InvokeClientService : IInvokeClientServices
    {
        private Uri apiInvoke;

        private string apiInvokeend;

        public InvokeClientService(Uri urlApi)
        {
            apiInvoke = urlApi;
        }

        public void ApiInvoke(string urlApi)
        {
            apiInvoke = new Uri(urlApi);
        }

        private void Addparameter(System.Net.Http.HttpClient httpCliente, Dictionary<string, string> paramHeadersInvoke)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in paramHeadersInvoke)
            {
                parameters[param.Key] = param.Value;
            }

            var builder = new UriBuilder(apiInvoke)
            {
                Query = parameters.ToString()
            };

            apiInvokeend = builder.ToString();
        }

        public async Task<TR> GetAsync<TR>() where TR : class, new()
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var response = await httpClient.GetAsync(apiInvoke);
                if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadAsStringAsync().ConfigureAwait(false)).ToJsonDeserialize<TR>(new JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }
                else { throw new Exception($"No obtuvo una respuesta exitosa del api {apiInvoke}: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}."); }
            }
        }

        public async Task<TR> GetAsync<TR>(Dictionary<string, string> paramHeadersInvoke) where TR : class, new()
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                Addparameter(httpClient, paramHeadersInvoke);
                var response = await httpClient.GetAsync(apiInvokeend);
                if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadAsStringAsync().ConfigureAwait(false)).ToJsonDeserialize<TR>(new JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }
                else { throw new Exception($"No obtuvo una respuesta exitosa del api {apiInvoke}: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}."); }
            }
        }

        public Task DeleteAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest content)
        {
            throw new NotImplementedException();
        }

        public async Task<TR> PostAsync<T, TR>(T objectTransferencia)
            where T : class, new()
            where TR : class, new()
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var httpPeticion = new StringContent(objectTransferencia.ToJsonSerialize(new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }), Encoding.UTF8, "application/json");


                Console.WriteLine(" Vpc url resultado: " + apiInvoke);
                Console.WriteLine(" Vpc httpPeticion resultado: " + httpPeticion);


                var response = await httpClient.PostAsync(apiInvoke, httpPeticion);

                Console.WriteLine(" Vpc objectTransferencia resultado: " + response);

                if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadAsStringAsync().ConfigureAwait(false)).ToJsonDeserialize<TR>(new JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }
                else { throw new Exception($"No obtuvo una respuesta exitosa del api {apiInvoke}: {await response.Content.ReadAsStringAsync().ConfigureAwait(false)}."); }
            }
        }
    }
}
