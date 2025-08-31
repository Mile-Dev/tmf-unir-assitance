using AutoMapper;
using EventServices.Common;
using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Dto.Query;
using EventServices.Domain.Entities;
using EventServices.Infraestructura.AuditLog;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Services.Interfaces;
using SharedServices.Objects;

namespace EventServices.Services
{
    /// <summary>
    /// Servicio para gestionar las consultas telefónicas programadas asociadas a eventos.
    /// Permite crear, cerrar y listar consultas telefónicas.
    /// </summary>
    public class PhoneConsultationServices(IUnitOfWork unitOfWork, ILogger<PhoneConsultationServices> logger, IMapper mapper, IAuditLogService auditLogService) : IPhoneConsultationServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<PhoneConsultationServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IAuditLogService _auditLogService = auditLogService;

        /// <summary>
        /// Cancela una consulta telefónica programada que no se cerro.
        /// </summary>
        /// <param name="id">Identificador de la consulta telefónica.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        /// <exception cref="Exception">Si la consulta no existe, ya está cancelada.</exception>
        public async Task<bool> CanceledScheduledPhoneConsultationAsync(int id)
        {
            var phoneConsultationObject = await _unitOfWork.PhoneConsultationRepository.GetByIdAsync(id);
            if (phoneConsultationObject == null)
            {
                _logger.LogError("Phone consultation with ID {Id} not found", id);
                throw new Exception($"Phone consultation with ID {id} not found");
            }

            if (phoneConsultationObject.Status == EnumStatusPhoneConsultation.Canceled.ToString())
            {
                _logger.LogError("Invalid operation: the phone consultation with ID {Id} is already canceled.", id);
                throw new Exception("This phone consultation has already been canceled.");
            }

            phoneConsultationObject.Status = EnumStatusPhoneConsultation.Canceled.ToString();
            phoneConsultationObject.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.PhoneConsultationRepository.UpdateAsync(phoneConsultationObject);
            await _unitOfWork.CompleteAsync();

            var eventObject = await _unitOfWork.EventsRepository.GetEventLogProjectionByIdAsync(phoneConsultationObject.EventId);
            var eventCode = eventObject?.ClientCode + "-" + eventObject?.Id.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_PHONECONSULTATION_CANCELED, eventObject.Id, eventObject.VoucherName);
            await _auditLogService.SendEventLogAsync(new RequestLogData
            {
                EventId = eventObject!.Id,
                Action = EnumActions.EVENT_PHONECONSULTATION_CANCELED.ToString(),
                Description = description,
                EventCode = eventCode,
                Role = Constans.MessageRole,
                SourceId = eventObject.ClientId.ToString(),
                SourceCode = eventObject?.ClientCode,
                StatusEvent = eventObject?.EventStatusName,
                StatusEventId = eventObject?.EventStatusId.ToString(),
                UserName = Constans.MessageUserName,
                SendToClient = !Constans.MESSAGE_SEND
            });

            return true;
        }

        /// <summary>
        /// Cierra una consulta telefónica programada estableciendo la fecha y hora de finalización.
        /// </summary>
        /// <param name="id">Identificador de la consulta telefónica.</param>
        /// <param name="phoneConsultation">DTO con la información de cierre de la consulta.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        /// <exception cref="Exception">Si la consulta no existe, ya está cerrada o la fecha de cierre es inválida.</exception>
        public async Task<bool> ClosedScheduledPhoneConsultationAsync(int id)
        {
            var phoneConsultationObject = await _unitOfWork.PhoneConsultationRepository.GetByIdAsync(id);
            if (phoneConsultationObject == null)
            {
                _logger.LogError("Phone consultation with ID {Id} not found", id);
                throw new Exception($"Phone consultation with ID {id} not found");
            }

            if (phoneConsultationObject.ScheduledEndAt != default)
            {
                _logger.LogError("Phone consultation with ID {Id} is already closed", id);
                throw new Exception($"Phone consultation with ID {id} is already closed");
            }

            phoneConsultationObject.ScheduledEndAt = DateTime.UtcNow;
            phoneConsultationObject.Status = EnumStatusPhoneConsultation.Completed.ToString();
            phoneConsultationObject.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.PhoneConsultationRepository.UpdateAsync(phoneConsultationObject);
            await _unitOfWork.CompleteAsync();

            var eventObject = await _unitOfWork.EventsRepository.GetEventLogProjectionByIdAsync(phoneConsultationObject.EventId);
            var EventCode = eventObject?.ClientCode + "-" + eventObject?.Id.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_PHONECONSULTATION_RESOLVE, eventObject.Id, eventObject.VoucherName);
            await _auditLogService.SendEventLogAsync(new RequestLogData
            {
                EventId = eventObject!.Id,
                Action = EnumActions.EVENT_PHONECONSULTATION_RESOLVE.ToString(),
                Description = description,
                EventCode = EventCode,
                Role = Constans.MessageRole,
                SourceId = eventObject.ClientId.ToString(),
                SourceCode = eventObject?.ClientCode,
                StatusEvent = eventObject?.EventStatusName,
                StatusEventId = eventObject?.EventStatusId.ToString(),
                UserName = Constans.MessageUserName,
                SendToClient = !Constans.MESSAGE_SEND
            });

            return true;
        }

        /// <summary>
        /// Crea una nueva consulta telefónica programada asociada a un evento.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <param name="phoneConsultation">DTO con la información de la consulta telefónica.</param>
        /// <returns>DTO con la información de la consulta creada.</returns>
        /// <exception cref="Exception">Si el evento no existe.</exception>
        public async Task<ResponseCreatedDto> CreatedScheduledPhoneConsultationAsync(int eventId, PhoneConsultationDto phoneConsultation)
        {
            var eventObject = await _unitOfWork.EventsRepository.GetEventLogProjectionByIdAsync(eventId);
            if (eventObject == null)
            {
                _logger.LogError("Event with ID {EventId} not found", eventId);
                throw new Exception("event not found");
            }

            var eventProviderObject = await _unitOfWork.EventProviderRepository.GetByIdAsync(phoneConsultation.EventProviderId);
            if (eventProviderObject == null || (eventProviderObject.EventId != eventId) )
            {
                _logger.LogError("Event Provider with ID {EventProviderId} not found in the event with Id {eventId}", phoneConsultation.EventProviderId, eventId);
                throw new Exception("Event Provider not found");
            }


            var existRecordsScheduled = await _unitOfWork.PhoneConsultationRepository.ExistsRecordScheduledAsync(eventId);
            if (existRecordsScheduled)
            {
                _logger.LogError("A scheduled phone consultation already exists for event ID {EventId}", eventId);
                throw new Exception("A scheduled phone consultation already exists for this event.");
            }

            var dateNow = DateTime.UtcNow                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             ;
            var phoneConsultationEntity = _mapper.Map<PhoneConsultation>(phoneConsultation);
            phoneConsultationEntity.Status = EnumStatusPhoneConsultation.Scheduled.ToString();
            phoneConsultationEntity.EventId = eventId;
            phoneConsultationEntity.PhoneConsultationSk = $"{dateNow:yyyyMMddHHmmss}";
            phoneConsultationEntity.CreatedAt = dateNow;
            phoneConsultationEntity.UpdatedAt = dateNow;

            var idPhoneConsultation = await _unitOfWork.PhoneConsultationRepository.AddAsync(phoneConsultationEntity); 
            await _unitOfWork.CompleteAsync();

            var EventCode = eventObject?.ClientCode + "-" + eventObject?.Id.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_PHONECONSULTATION_CREATE, eventObject.Id, eventObject.VoucherName);
            await _auditLogService.SendEventLogAsync(new RequestLogData
            {
                EventId = eventObject!.Id,
                Action = EnumActions.EVENT_PHONECONSULTATION_CREATE.ToString(),
                Description = description,
                EventCode = EventCode,
                Role = Constans.MessageRole,
                SourceId = eventObject.ClientId.ToString(),
                SourceCode = eventObject?.ClientCode,
                StatusEvent = eventObject?.EventStatusName,
                StatusEventId = eventObject?.EventStatusId.ToString(),
                UserName = Constans.MessageUserName,
                SendToClient = !Constans.MESSAGE_SEND
            });

            return new ResponseCreatedDto
            {
                Id = idPhoneConsultation.Id,
                CodeEvent = EventCode,
                CreatedAt = dateNow
            };
        }

        /// <summary>
        /// Obtiene la lista de consultas telefónicas asociadas a un evento.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <returns>Lista de consultas telefónicas.</returns>
        public async Task<List<PhoneConsultationQueryDto>> GetListPhoneConsultationsByEventAsync(int eventId)
        {
            var consultations = await _unitOfWork.PhoneConsultationRepository.ListPhoneConsultationAsync(eventId);
            return _mapper.Map<List<PhoneConsultationQueryDto>>(consultations);
        }

        /// <summary>
        /// Reprograma la fecha y hora de una consulta telefónica programada existente.
        /// </summary>
        /// <param name="id">Identificador de la consulta telefónica a reprogramar.</param>
        /// <param name="reschedulePhoneConsultation">Datos con la nueva fecha y hora de la consulta.</param>
        /// <returns>True si la reprogramación fue exitosa, false en caso contrario.</returns>
        public async Task<bool> ReScheduledPhoneConsultationAsync(int id, ReschedulePhoneConsultationDto reschedulePhoneConsultation)
        {
            var eventPhoneConsultation = await _unitOfWork.PhoneConsultationRepository.GetByIdAsync(id);
            if (eventPhoneConsultation == null)
            {
                _logger.LogError("Event Phone Consultation not found");
                return false;
            }

            string action = EnumActions.EVENT_PHONECONSULTATION_RESCHEDULE.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_PHONECONSULTATION_RESCHEDULE, eventPhoneConsultation.EventId);
            await SendEventLogAsync(eventPhoneConsultation.EventId, action, description, !Constans.MESSAGE_SEND);

            eventPhoneConsultation.ScheduledAt = reschedulePhoneConsultation.ScheduledAt;
            await _unitOfWork.PhoneConsultationRepository.UpdateAsync(eventPhoneConsultation);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        /// <summary>
        /// Envía un registro de auditoría al sistema de logs para un evento específico,
        /// utilizando la acción de "reagendamiento" (reschedule).
        /// Obtiene la proyección de log del evento por su identificador, construye el código del evento
        /// y envía la información relevante al servicio de auditoría.
        /// </summary>
        /// <param name="eventId">Identificador del evento asociado al proveedor.</param>
        /// <param name="action">Identificador del evento asociado al proveedor.</param>
        /// <param name="description">Identificador del evento asociado al proveedor.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        private async Task SendEventLogAsync(int eventId, string action, string description, bool sendtoclient)
        {
            var eventObject = await _unitOfWork.EventsRepository.GetEventLogProjectionByIdAsync(eventId);
            var EventCode = eventObject?.ClientCode + "-" + eventObject?.Id.ToString();
            await _auditLogService.SendEventLogAsync(new RequestLogData
            {
                EventId = eventObject!.Id,
                Action = action,
                Description = description,
                EventCode = EventCode,
                Role = Constans.MessageRole,
                SourceId = eventObject.ClientId.ToString(),
                SourceCode = eventObject?.ClientCode,
                StatusEvent = eventObject?.EventStatusName,
                StatusEventId = eventObject?.EventStatusId.ToString(),
                UserName = Constans.MessageUserName,
                SendToClient = sendtoclient
            });
        }
    }
}
