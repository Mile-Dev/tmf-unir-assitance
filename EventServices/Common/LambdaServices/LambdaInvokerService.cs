using Amazon.Lambda;
using Amazon.Lambda.Model;
using System.Text.Json;

namespace EventServices.Common.LambdaServices
{
    public class LambdaInvokerService(IAmazonLambda lambdaClient, ILogger<LambdaInvokerService> logger)
    {
        private readonly IAmazonLambda _lambdaClient = lambdaClient;
        private readonly ILogger<LambdaInvokerService> _logger = logger;

        public async Task<string> InvokeAsync(string functionName, object payload)
        {

            var jsonPayload = JsonSerializer.Serialize(payload);
            _logger.LogInformation("InvokeAsync: {@InvokeAsync}", jsonPayload);

            var request = new InvokeRequest
            {
                FunctionName = functionName,
                Payload = jsonPayload
            };

            _logger.LogInformation("***#Invokerequest: {@InvokeAsync}", request.FunctionName);
            _logger.LogInformation("***#Invokerequest: {@InvokeAsync}", request.Payload);

            try
            {
                var response = await _lambdaClient.InvokeAsync(request);
                _logger.LogInformation("***responseInvokeAsync: {@response}", response);

                if (response.FunctionError != null)
                {
                    _logger.LogError($"Error al invocar la función: {response.FunctionError}");
                    throw new Exception($"La función invocada devolvió un error: {response.FunctionError}");
                }

                using (var streamReader = new StreamReader(response.Payload))
                {
                    return await streamReader.ReadToEndAsync();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error invoking lambda function {functionName}");
                throw;
            }
        }
    }
}