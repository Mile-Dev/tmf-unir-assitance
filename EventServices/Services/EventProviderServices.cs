using AutoMapper;
using EventServices.Common;
using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Infraestructura.AuditLog;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Services.Interfaces;
using SharedServices.Objects;

namespace EventServices.Services
{
    /// <summary>
    /// Servicio para la gestión de proveedores de eventos, incluyendo operaciones CRUD y envío de logs a SQS.
    /// </summary>
    public class EventProviderServices(IUnitOfWork unitOfWork, ILogger<EventProviderServices> logger, IMapper mapper, IAuditLogService auditLogService) : IEventProviderServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<EventProviderServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IAuditLogService _auditLogService = auditLogService;

        /// <summary>
        /// Crea un nuevo proveedor de evento y registra la acción en SQS.
        /// </summary>
        /// <param name="eventProviderDto">DTO con la información del proveedor de evento.</param>
        /// <returns>DTO con el identificador y código del evento creado.</returns>
        /// <exception cref="Exception">Si no se encuentra el subtipo de asistencia o el evento.</exception>
        public async Task<ResponseCreatedDto> CreatedEventProviderAsync(EventProviderDto eventProviderDto)
        {
            var generalTypesObject = await _unitOfWork.GeneralTypesRepository.GetByIdAsync(eventProviderDto.AssistanceSubTypeId);
            if (generalTypesObject == null)
            {
                _logger.LogError("Sub type not found");
                throw new Exception("Sub type not found");
            }

            var eventObject = await _unitOfWork.EventsRepository.GetEventLogProjectionByIdAsync(eventProviderDto.EventId);
            if (eventObject == null)
            {
                _logger.LogError("event not found");
                throw new Exception("event not found");
            }

            List<string> listCodes =
            [
                Constans.STATUS_EP_APPOINTMENT_SCHEDULED,
                Constans.STATUS_EP_NO_PROVIDER_ASSIGNED,
            ];

            var listsStatusEventProvider = await _unitOfWork.EventProviderStatusRepository.GetManyByCodesAsync(listCodes);
            var eventProvider = _mapper.Map<Domain.Entities.EventProvider>(eventProviderDto);
            eventProvider.EventProviderStatusId = eventProviderDto.ScheduledAppointment != null ?
                                                  listsStatusEventProvider.FirstOrDefault(item => item.Code == Constans.STATUS_EP_APPOINTMENT_SCHEDULED)!.Id :
                                                  listsStatusEventProvider.FirstOrDefault(item => item.Code == Constans.STATUS_EP_NO_PROVIDER_ASSIGNED)!.Id;
            eventProvider.CreatedAt = DateTime.Now;
            eventProvider.UpdatedAt = DateTime.Now;
            var eventObjectAdd = await _unitOfWork.EventProviderRepository.AddAsync(eventProvider);
            await _unitOfWork.CompleteAsync();

            var eventCode = eventObject?.ClientCode + "-" + eventObject?.Id.ToString();
            var codestatus = eventObject?.EventStatusId.ToString() ?? string.Empty;

            var description = string.Format(Constans.MESSAGE_EVENT_PROVIDER_CREATE, eventObjectAdd.Id, eventObjectAdd.EventId, eventObject.VoucherName,
                                            generalTypesObject.Name, eventObject.EventStatusName );

            await _auditLogService.SendEventLogAsync(new RequestLogData
            {
                EventId = eventObject!.Id,
                Action = EnumActions.EVENT_PROVIDER_CREATE.ToString(),
                Description = description,
                EventCode = eventCode,
                Role = Constans.MessageRole,
                SourceId = eventObject.ClientId.ToString(),
                SourceCode = eventObject?.ClientCode,
                StatusEvent = eventObject?.EventStatusName,
                StatusEventId = eventObject?.EventStatusId.ToString(),
                UserName = Constans.MessageUserName,
                SendToClient = Constans.MESSAGE_SEND
            });

            ResponseCreatedDto responseCreatedDto = new()
            {
                Id = eventObjectAdd.Id,
                CodeEvent = eventCode,
            };

            return responseCreatedDto;
        }

        /// <summary>
        /// Elimina de forma permanente un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>True si se eliminó correctamente, false si no se encontró.</returns>
        public async Task<bool> DeletedEventProviderByIdAsync(int id)
        {
            var eventProvider = await _unitOfWork.EventProviderRepository.GetByIdAsync(id);
            if (eventProvider == null)
            {
                _logger.LogError("EventProvider not found");
                return false;
            }

            await _unitOfWork.EventProviderRepository.HardDelete(id);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        /// <summary>
        /// Obtiene la lista de proveedores de evento asociados a un evento específico.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Lista de DTOs de proveedores de evento.</returns>
        public async Task<List<Domain.Dto.Query.EventProviderDto>> GetEventProviderByEventIdAsync(int id)
        {
            var eventObject = await _unitOfWork.EventProviderRepository.GetEventProviderByEventIdAsync(id);
            var eventProvider = _mapper.Map<List<Domain.Dto.Query.EventProviderDto>>(eventObject);
            return eventProvider;
        }

        /// <summary>
        /// Obtiene un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>DTO del proveedor de evento.</returns>
        /// <exception cref="Exception">Si no se encuentra el proveedor de evento.</exception>
        public async Task<Domain.Dto.Query.EventProviderDto> GetEventProviderByIdAsync(int id)
        {
            var eventObject = await _unitOfWork.EventProviderRepository.GetEventProviderByIdAsync(id);
            if (eventObject == null)
            {
                _logger.LogError("EventProvider not found");
                throw new Exception("Event provider not found");
            }
            var eventProvider = _mapper.Map<Domain.Dto.Query.EventProviderDto>(eventObject);
            return eventProvider;
        }

        /// <summary>
        /// Actualiza la información de un proveedor de evento existente y registra la acción en SQS.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="eventProviderDto">DTO con la información actualizada.</param>
        /// <returns>DTO actualizado del proveedor de evento.</returns>
        /// <exception cref="Exception">Si no se encuentra el proveedor de evento.</exception>
        public async Task<Domain.Dto.Query.EventProviderDto> UpdatedEventProviderAsync(int id, EventProviderDto eventProviderDto)
        {
            var eventProviderObject = await _unitOfWork.EventProviderRepository.GetEventProviderByIdAsync(id);
            if (eventProviderObject == null)
            {
                _logger.LogError("EventProvider not found");
                throw new Exception("Event provider not found");
            }

            List<string> listCodes =
            [
               Constans.STATUS_EP_APPOINTMENT_SCHEDULED,
               Constans.STATUS_EP_NO_PROVIDER_ASSIGNED,
            ];
            var listsStatusEventProvider = await _unitOfWork.EventProviderStatusRepository.GetManyByCodesAsync(listCodes);

            var eventProvider = _mapper.Map<Domain.Entities.EventProvider>(eventProviderDto);
            eventProvider.Id = id;
            eventProvider.EventId = eventProviderObject.EventId;
            eventProvider.EventProviderStatusId = eventProviderDto.ScheduledAppointment != null ?
                                      listsStatusEventProvider.FirstOrDefault(item => item.Code == Constans.STATUS_EP_APPOINTMENT_SCHEDULED)!.Id :
                                      listsStatusEventProvider.FirstOrDefault(item => item.Code == Constans.STATUS_EP_NO_PROVIDER_ASSIGNED)!.Id;
            eventProvider.ExternalRequestId = eventProviderDto.ExternalRequestId ?? eventProviderObject.ExternalRequestId;
            eventProvider.ExternalData = eventProviderObject.ExternalData;
            eventProvider.UpdatedAt = DateTime.UtcNow;
            eventProvider.CreatedAt = eventProviderObject.CreatedAt;
            var eventProviderUpdate = await _unitOfWork.EventProviderRepository.UpdateAsync(eventProvider);
            await _unitOfWork.CompleteAsync();

            string action = EnumActions.EVENT_PROVIDER_UPDATE.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_PROVIDER_UPDATE, eventProvider.EventId);
            bool sendtoclient = !Constans.MESSAGE_SEND;
            await SendEventLogAsync(eventProvider.EventId, action, description, sendtoclient);

            var eventProviderResult = _mapper.Map<Domain.Dto.Query.EventProviderDto>(eventProviderUpdate);

            return eventProviderResult;
        }

        /// <summary>
        /// Cancela un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>True si se canceló correctamente, false si no se encontró.</returns>
        public async Task<Domain.Dto.Query.EventProviderDto> CanceledEventProviderByIdAsync(int id)
        {
            var eventProvider = await _unitOfWork.EventProviderRepository.GetEventProviderByIdAsync(id);
            if (eventProvider == null)
            {
                _logger.LogError("EventProvider not found");
                throw new Exception("Event provider not found");
            }

            var statusId = await _unitOfWork.EventProviderStatusRepository.GetStatusIdByCodeAsync(Constans.STATUS_EP_SERVICE_CANCELED);
            eventProvider.EventProviderStatusId = statusId;
            await _unitOfWork.EventProviderRepository.UpdateAsync(eventProvider);
            await _unitOfWork.CompleteAsync();

            string action = EnumActions.EVENT_PROVIDER_CANCELED.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_PROVIDER_CANCELED, eventProvider.EventId);
            bool sendtoclient = !Constans.MESSAGE_SEND;
            await SendEventLogAsync(eventProvider.EventId, action, description, sendtoclient);
            var eventProviderDto = _mapper.Map<Domain.Dto.Query.EventProviderDto>(eventProvider);
            return eventProviderDto;
        }

        /// <summary>
        /// Marca como completado un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>True si se completó correctamente, false si no se encontró.</returns>
        public async Task<Domain.Dto.Query.EventProviderDto> CompletedEventProviderByIdAsync(int id)
        {
            var eventProvider = await _unitOfWork.EventProviderRepository.GetEventProviderByIdAsync(id);
            if (eventProvider == null)
            {
                _logger.LogError("EventProvider not found");
                throw new Exception("Event provider not found");
            }
            var assistencename = await _unitOfWork.GeneralTypesRepository.GetByIdAsync(eventProvider.AssistanceSubTypeId);
            var statusId = await _unitOfWork.EventProviderStatusRepository.GetStatusIdByCodeAsync(Constans.STATUS_EP_SERVICE_COMPLETED);
            eventProvider.EventProviderStatusId = statusId;
            await _unitOfWork.EventProviderRepository.UpdateAsync(eventProvider);
            await _unitOfWork.CompleteAsync();

            var description = string.Format(Constans.MESSAGE_EVENT_PROVIDER_RESOLVE, eventProvider.EventId, assistencename!.Name, eventProvider.DiagnosisIcd);
            string action = EnumActions.EVENT_PROVIDER_RESOLVE.ToString();
            bool sendtoclient = Constans.MESSAGE_SEND;
            await SendEventLogAsync(eventProvider.EventId, action, description, sendtoclient);
            var eventProviderDto = _mapper.Map<Domain.Dto.Query.EventProviderDto>(eventProvider);
            return eventProviderDto;
        }

        /// <summary>
        /// Reagenda un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="rescheduleEventProvider">Datos para el reagendamiento.</param>
        /// <returns>True si se reagendó correctamente, false si no se encontró.</returns>
        public async Task<Domain.Dto.Query.EventProviderDto> RescheduledEventProviderByIdAsync(int id, RescheduleEventProviderDto rescheduleEventProvider)
        {
            var eventProvider = await _unitOfWork.EventProviderRepository.GetEventProviderByIdAsync(id);
            if (eventProvider == null)
            {
                _logger.LogError("EventProvider not found");
                throw new Exception("Event provider not found");
            }

            var statusId = await _unitOfWork.EventProviderStatusRepository.GetStatusIdByCodeAsync(Constans.STATUS_EP_APPOINTMENT_SCHEDULED);
            eventProvider.EventProviderStatusId = statusId;
            eventProvider.ScheduledAppointment = rescheduleEventProvider.ScheduledAppointment;
            await _unitOfWork.EventProviderRepository.UpdateAsync(eventProvider);
            await _unitOfWork.CompleteAsync();

            string action = EnumActions.EVENT_PROVIDER_RESCHEDULE.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_PROVIDER_RESCHEDULE, eventProvider.EventId);
            bool sendtoclient = !Constans.MESSAGE_SEND;
            await SendEventLogAsync(eventProvider.EventId, action, description, sendtoclient);
            var eventProviderDto = _mapper.Map<Domain.Dto.Query.EventProviderDto>(eventProvider);
            return eventProviderDto;
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
            var eventCode = eventObject?.ClientCode + "-" + eventObject?.Id.ToString();

            await _auditLogService.SendEventLogAsync(new RequestLogData
            {
                EventId = eventObject!.Id,
                Action = action,
                Description = description,
                EventCode = eventCode,
                Role = Constans.MessageRole,
                SourceId = eventObject.ClientId.ToString(),
                SourceCode = eventObject?.ClientCode,
                StatusEvent = eventObject?.EventStatusName,
                StatusEventId = eventObject?.EventStatusId.ToString(),
                UserName = Constans.MessageUserName,
                SendToClient = sendtoclient
            });
        }

        /// <summary>
        /// Actualiza el diagnóstico de un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="diagnostic">Nuevo diagnóstico a asignar.</param>
        /// <returns>DTO con la información actualizada del proveedor de evento.</returns>
        public async Task<Domain.Dto.Query.EventProviderDto> UpdateDiagnosticEventProviderByIdAsync(int id, RequestUpdateDiagnosticDto diagnostic)
        {
            var eventProvider = await _unitOfWork.EventProviderRepository.GetEventProviderByIdAsync(id);
            if (eventProvider == null)
            {
                _logger.LogError("EventProvider not found");
                throw new Exception("Event provider not found");
            }

            eventProvider.DiagnosisIcd = diagnostic.Diagnostic;
            await _unitOfWork.EventProviderRepository.UpdateAsync(eventProvider);
            await _unitOfWork.CompleteAsync();

            string action = EnumActions.EVENT_PROVIDER_UPDATE.ToString();
            var description = string.Format(Constans.MESSAGE_EVENT_PROVIDER_UPDATE_DIAGNOSTIC, eventProvider.EventId, eventProvider.DiagnosisIcd);
            bool sendtoclient = !Constans.MESSAGE_SEND;
            await SendEventLogAsync(eventProvider.EventId, action, description, sendtoclient);
            var eventProviderDto = _mapper.Map<Domain.Dto.Query.EventProviderDto>(eventProvider);

            return eventProviderDto;
        }
    }
}
