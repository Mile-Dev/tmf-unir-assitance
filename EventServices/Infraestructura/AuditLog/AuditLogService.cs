using EventServices.Domain.Dto;
using SQSProducerServices.Common.Interfaces;

namespace EventServices.Infraestructura.AuditLog
{
    /// <summary>
    /// Servicio encargado de enviar logs de auditoría a una cola SQS de AWS.
    /// </summary>
    public class AuditLogService(ISqsService sqsService, ILogger<AuditLogService> logger, IConfiguration configuration) : IAuditLogService
    {
        // Servicio para interactuar con SQS.
        private readonly ISqsService _sqsService = sqsService;

        // Logger para registrar información y errores.
        private readonly ILogger<AuditLogService> _logger = logger;
        
        // URL de la cola SQS configurada.
        private readonly string _queueUrl = configuration["AWS:SQS:QueueUrl"]
                ?? throw new ArgumentNullException("SQS queue URL not configured.");

        /// <summary>
        /// Envía un log de evento a la cola SQS configurada.
        /// </summary>
        /// <param name="logData">Datos del log a enviar.</param>
        public async Task SendEventLogAsync(RequestLogData logData)
        {
            _logger.LogInformation("Sending audit log to SQS: {@LogData}", logData);
            await _sqsService.SendMessageAsync(logData, _queueUrl);
        }
    }
}
