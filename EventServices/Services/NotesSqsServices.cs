using EventServices.Domain.Dto;
using EventServices.Services.Interfaces;
using SharedServices.Objects;
using SQSProducerServices.Common.Interfaces;

namespace EventServices.Services
{
    public class NotesSqsServices(ISqsService sqsService, ILogger<NotesSqsServices> logger,  IConfiguration _configuration) : INotesSqsServices
    {
        private readonly ISqsService _sqsService = sqsService;
        private readonly string queueUrl = _configuration["AWS:SQS:QueueUrl"]!;
        private readonly ILogger<NotesSqsServices> _logger = logger;

        public async Task SendMessageAsync(RequestLogData message)
        {
            try
            {
                _logger.LogInformation("Se va a enviar el mensaje a la cola: {Message}", message.EventId);
                message.Action = EnumActions.NOTE_REGISTER.ToString();
                await _sqsService.SendMessageAsync(message, queueUrl);
                _logger.LogInformation("Ya se envio el mensaje a la cola: {Message}", message.EventId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while geting the GetEventDetailsByCode: {Message}", ex.Message);
                throw new Exception($"Error sending message to SQS: {ex.Message}", ex);
            }
        }
    }
}
