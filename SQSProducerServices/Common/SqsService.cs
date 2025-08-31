using Amazon.SQS;
using Amazon.SQS.Model;
using SQSProducerServices.Common.Interfaces;
using System.Text.Json;

public class SqsService(IAmazonSQS sqsClient) : ISqsService
{
    private readonly IAmazonSQS _sqsClient = sqsClient;

    public async Task SendMessageAsync<T>(T message, string queueUrl)
    {
        var jsonMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = jsonMessage
        };

        await _sqsClient.SendMessageAsync(request);
    }
}
