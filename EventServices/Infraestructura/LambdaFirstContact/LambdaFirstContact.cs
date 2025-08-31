
using Amazon.Lambda;
using EventServices.Common.LambdaServices;

namespace EventServices.Infraestructura.LambdaFirstContact
{
    public class LambdaFirstContact(IAmazonLambda lambdaClient, ILogger<LambdaFirstContact> logger) : LambdaInvokerService(lambdaClient, logger), ILambdaFirstContact
    {
        private readonly ILogger<LambdaFirstContact> _logger = logger;
        public async Task<string> InvokeLambdaGetByIdAsync(string functionArn, string resourcePath, string path, string id)
        {
            _logger.LogInformation("InvokeLambdaGetByIdAsync: {@InvokeLambdaGetByIdAsync}", "LLego hasta aqui LambdaFirstContact");

            var payload = new
            {
                resource = resourcePath,
                path,
                httpMethod = "GET",
                pathParameters = new { id },
                headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            }
            };
            _logger.LogInformation("LambdaFirstContact payload: {@payload}", payload);

            return await InvokeAsync(functionArn, payload);
        }
    }
}
