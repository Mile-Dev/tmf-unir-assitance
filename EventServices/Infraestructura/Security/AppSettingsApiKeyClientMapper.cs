using EventServices.Services.Interfaces.Security;
using Microsoft.Extensions.Options;

namespace EventServices.Infraestructura.Security
{
    public class AppSettingsApiKeyClientMapper(IOptions<ClientKeyApiMappingOptions> options) : IApiKeyClientMapper
    {
        private readonly Dictionary<string, List<string>> _clientKeyMappings = options.Value;

        public string? ResolveClientKey(string apiKey)
        {
            foreach (var kvp in _clientKeyMappings)
            {
                if (kvp.Value.Contains(apiKey))
                    return kvp.Key; 
            }

            return null;
        }
    }
}
