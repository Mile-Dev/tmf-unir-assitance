using EventServices.Domain.Dto;

namespace EventServices.Infraestructura.AuditLog
{
    /// <summary>
    /// Define la interfaz para el servicio de registro de auditoría.
    /// Proporciona métodos para enviar información de eventos al sistema de logs.
    /// </summary>
    public interface IAuditLogService
    {
        /// <summary>
        /// Envía de manera asíncrona un registro de evento al sistema de auditoría.
        /// </summary>
        /// <param name="logData">Datos del evento a registrar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task SendEventLogAsync(RequestLogData logData);
    }
}
