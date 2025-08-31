using EventServices.Common;
using EventServices.Domain.Dto;
using EventServices.Domain.Entities;
using EventServices.EventFirstContact.Domain.Entities;
using EventServices.Infraestructura.AuditLog;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Services.Interfaces;
using SharedServices.Objects;

namespace EventServices.Services
{
    /// <summary>
    /// Servicio para gestionar los cambios de estado de los eventos.
    /// Proporciona métodos para cancelar, completar y actualizar el estado de un evento.
    /// </summary>
    public class EventStatusChangeServices(IUnitOfWork unitOfWork, ILogger<EventStatusChangeServices> logger, IAuditLogService auditLogService) : IEventStatusChangeServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<EventStatusChangeServices> _logger = logger;
        private readonly IAuditLogService _auditLogService = auditLogService;

        /// <summary>
        /// Cancela un evento específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador del evento a cancelar.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        public async Task<bool> CancelEventAsync(int id)
        {
            var eventObject = await _unitOfWork.EventsRepository.GetByIdAsync(id);
            if (eventObject == null)
            {
                _logger.LogError("Event with ID {Id} not found", id);
                throw new Exception($"Event  with ID {id} not found");
            }

            var statusId = await _unitOfWork.EventStatusRepository.GetStatusIdByCodeAsync(Constans.STATUS_EVENT_CANCEL);
            eventObject.EventStatusId = statusId;
            eventObject.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.EventsRepository.UpdateAsync(eventObject);
            await _unitOfWork.CompleteAsync();

            string action = EnumActions.EVENT_CANCELED.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_CANCELED, eventObject.Id, eventObject.Voucher_Name);
            await SendEventLogChangedStatusAsync(eventObject, action, description, !Constans.MESSAGE_SEND);

            return true;
        }

        /// <summary>
        /// Marca un evento como completado por su identificador.
        /// </summary>
        /// <param name="id">Identificador del evento a completar.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        public async Task<bool> CompleteEventAsync(int id)
        {
            var eventObject = await _unitOfWork.EventsRepository.GetByIdAsync(id);
            if (eventObject == null)
            {
                _logger.LogError("Event with ID {Id} not found", id);
                throw new Exception($"Event  with ID {id} not found");
            }

            var statusId = await _unitOfWork.EventStatusRepository.GetStatusIdByCodeAsync(Constans.STATUS_EVENT_CLOSED);
            eventObject.EventStatusId = statusId;
            eventObject.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.EventsRepository.UpdateAsync(eventObject);
            await _unitOfWork.CompleteAsync();

            string action = EnumActions.EVENT_CLOSE.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_CLOSE, eventObject.Id, eventObject.Voucher_Name);
            await SendEventLogChangedStatusAsync(eventObject, action, description, !Constans.MESSAGE_SEND);

            return true;
        }


        /// <summary>
        /// Re abrir un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        public async Task<bool> ReOpenEventAsync(int id)
        {
            var eventObject = await _unitOfWork.EventsRepository.GetByIdAsync(id);
            if (eventObject == null)
            {
                _logger.LogError("Event with ID {Id} not found", id);
                throw new Exception($"Event  with ID {id} not found");
            }

            var statusId = await _unitOfWork.EventStatusRepository.GetStatusIdByCodeAsync(Constans.STATUS_EVENT_IN_ASSISTENCE_TEAM);
            eventObject.EventStatusId = statusId;
            eventObject.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.EventsRepository.UpdateAsync(eventObject);
            await _unitOfWork.CompleteAsync();

            string action = EnumActions.EVENT_REOPEN.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_REOPEN, eventObject.Id, eventObject.Voucher_Name);
            await SendEventLogChangedStatusAsync(eventObject, action, description, !Constans.MESSAGE_SEND);

            return true;
        }

        /// <summary>
        /// Actualiza el estado de un evento específico.
        /// </summary>
        /// <param name="Id">Identificador del evento.</param>
        /// <param name="eventStatus">Objeto con la información del nuevo estado.</param>
        /// <returns>Respuesta con el estado actualizado del evento.</returns>
        /// <exception cref="ArgumentNullException">Se lanza si el evento o el estado no existen.</exception>
        public async Task<ResponseEventStatus> UpdatedStatusOnEventAsync(int Id, RequestEventStatus eventStatus)
        {
            if (eventStatus == null)
                throw new ArgumentNullException(nameof(eventStatus), "The request cannot be null.");

            var responseEventStatus = await _unitOfWork.EventStatusRepository.GetByIdAsync(eventStatus.EventStatusId) ??
                                            throw new ArgumentNullException(nameof(eventStatus), "Event Status not found.");

            var responseEvent = await _unitOfWork.EventsRepository.GetByIdAsync(Id) ??
                                            throw new ArgumentNullException(nameof(eventStatus), "Event not found");

            responseEvent.EventStatusId = eventStatus.EventStatusId;
            responseEvent.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.EventsRepository.UpdateAsync(responseEvent);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Change to {EventStatusName} state is made in event {EventId}.", responseEventStatus.Name, responseEvent.Id);

            return new ResponseEventStatus
            {
                Id = responseEvent.Id,
                UpdatedAt = responseEvent.UpdatedAt,
                EventStatus = responseEventStatus.Name
            };
        }

        /// <summary>
        /// Envía un registro de auditoría al sistema de logs cuando cambia el estado de un evento.
        /// Construye el código del evento y recopila información relevante como el identificador, acción realizada,
        /// descripción, código del evento, rol, identificador y código de la fuente, estado actual y usuario.
        /// Utiliza el servicio de auditoría para registrar estos datos de manera asíncrona.
        /// </summary>
        /// <param name="eventObject">Objeto evento que ha cambiado de estado.</param>
        /// <param name="action">Acción realizada sobre el evento (por ejemplo, cerrar, reabrir).</param>
        /// <param name="description">Descripción detallada de la acción realizada.</param>
        private async Task SendEventLogChangedStatusAsync(Domain.Entities.Event? eventObject, string action, string description, bool sendtomessage)
        {
            var EventCode = eventObject?.VoucherNavigation?.Client.Code + "-" + eventObject?.Id.ToString();
            await _auditLogService.SendEventLogAsync(new RequestLogData
            {
                EventId = eventObject!.Id,
                Action = action,
                Description = description,
                EventCode = EventCode,
                Role = Constans.MessageRole,
                SourceId = eventObject?.VoucherNavigation?.Client?.Id.ToString(),
                SourceCode = eventObject?.VoucherNavigation?.Client?.Code?.ToString(),
                StatusEvent = eventObject?.EventStatusNavigation?.Name,
                StatusEventId = eventObject?.EventStatusId.ToString(),
                UserName = Constans.MessageUserName,
                SendToClient = sendtomessage
            });
        }
    }
}
