using AutoMapper;
using EventServices.Common;
using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Entities;
using EventServices.Infraestructura.AuditLog;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Services.Interfaces;
using SharedServices.Objects;

namespace EventServices.Services
{
    /// <summary>
    /// Servicio para la gestión de pagos de garantía, incluyendo operaciones de creación, actualización, cancelación, eliminación y consulta.
    /// </summary>
    public class GuaranteePaymentServices(IUnitOfWork unitOfWork, ILogger<GuaranteePaymentServices> logger, IMapper mapper, IAuditLogService auditLogService) : IGuaranteePaymentServices
    {
        /// <summary>
        // Declaración e inicialización del campo de solo lectura para la unidad de trabajo, que gestiona los repositorios y la transacción.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        // Declaración e inicialización del campo de solo lectura para el logger, utilizado para registrar información, advertencias y errores.
        /// </summary>
        private readonly ILogger<GuaranteePaymentServices> _logger = logger;

        /// <summary>
        // Declaración e inicialización del campo de solo lectura para el mapeador de objetos, que facilita la conversión entre entidades y DTOs.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        // Declaración e inicialización del campo de solo lectura para el servicio de auditoría, encargado de registrar logs de eventos relevantes.
        /// </summary>
        private readonly IAuditLogService _auditLogService = auditLogService;


        /// <summary>
        /// Cancela una garantía de pago por su identificador.
        /// </summary>
        /// <param name="id">Identificador del pago de garantía.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        /// <exception cref="Exception">Si no se encuentra la garantía de pago.</exception>
        public async Task<bool> CanceledGuaranteePaymentByIdAsync(int id)
        {
            var guaranteePayment = await _unitOfWork.GuaranteePaymentRepository.GetByIdAsync(id);
            if (guaranteePayment == null)
            {
                _logger.LogError("Guarantee Payment with Id {id} not found", id);
                throw new Exception($"Guarantee Payment with Id {id} not found");
            }
            guaranteePayment.GuaranteePaymentStatusId = Constans.CanceledGuaranteePayment;
            guaranteePayment.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.GuaranteePaymentRepository.UpdateAsync(guaranteePayment);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        /// <summary>
        /// Crea una nueva garantía de pago.
        /// </summary>
        /// <param name="GuaranteePaymentObject">Objeto con los datos de la garantía de pago.</param>
        /// <returns>DTO con el identificador de la garantía de pago creado.</returns>
        /// <exception cref="Exception">Si no se encuentra el proveedor de evento asociado.</exception>
        public async Task<ResponseCreatedDto> CreatedGuaranteePaymentAsync(GuaranteePaymentDto GuaranteePaymentObject)
        {
            var eventProviderDto = await _unitOfWork.EventProviderRepository.GetByIdAsync(GuaranteePaymentObject.EventProviderId);

            if (eventProviderDto == null)
            {
                _logger.LogError("EventProvider not found");
                throw new Exception("Event provider not found");
            }
            
            var guaranteePayment = _mapper.Map<GuaranteePayment>(GuaranteePaymentObject);
            guaranteePayment.GuaranteePaymentStatusId = 2;
            guaranteePayment.CreatedAt = DateTime.Now;
            guaranteePayment.UpdatedAt = DateTime.Now;
            var guaranteePaymentCreated = await _unitOfWork.GuaranteePaymentRepository.AddAsync(guaranteePayment);
            await _unitOfWork.CompleteAsync();

            var eventObject = await _unitOfWork.EventsRepository.GetEventLogProjectionByIdAsync(eventProviderDto.EventId);
            var eventCode = eventObject?.ClientCode + "-" + eventObject?.Id.ToString();
            var description = string.Format(Constans.MESSAGE_GUARANTEE_PAYMENT_CREATE, guaranteePaymentCreated.Id, eventObject!.Id, eventObject.VoucherName);
            await _auditLogService.SendEventLogAsync(new RequestLogData
            {
                EventId = eventObject!.Id,
                Action = EnumActions.GUARANTEE_PAYMENT_CREATE.ToString(),
                Description = description,
                EventCode = eventCode,
                Role = Constans.MessageRole,
                SourceId = eventObject?.ClientId.ToString(),
                SourceCode = eventObject?.ClientCode,
                StatusEvent = eventObject?.EventStatusName,
                StatusEventId = eventObject?.EventStatusId.ToString(),
                UserName = Constans.MessageUserName,
                SendToClient = Constans.MESSAGE_SEND
            });

            ResponseCreatedDto responseCreatedDto = new()
            {
                Id = guaranteePaymentCreated.Id,
                CodeEvent = eventCode,
                CreatedAt = guaranteePaymentCreated.CreatedAt,
            };

            return responseCreatedDto;
        }

        /// <summary>
        /// Elimina una garantía de pago por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la garantía de pago.</param>
        /// <returns>True si la eliminación fue exitosa, false si no se encontró el pago.</returns>
        public async Task<bool> DeletedGuaranteePaymentByIdAsync(int id)
        {
            var eventProvider = await _unitOfWork.GuaranteePaymentRepository.GetByIdAsync(id);
            if (eventProvider == null)
            {
                _logger.LogError("Guarantee Payment not found");
                return false;
            }
            await _unitOfWork.GuaranteePaymentRepository.HardDelete(id);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        /// <summary>
        /// Obtiene la información de una garantía de pago por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la garantía de pago.</param>
        /// <returns>DTO con la información de la garantía de pago.</returns>
        /// <exception cref="Exception">Si no se encuentra la garantía de pago.</exception>
        public async Task<Domain.Dto.Query.GuaranteePaymentDto> GetGuaranteePaymentAsync(int id)
        {
            var guaranteePayment = await _unitOfWork.GuaranteePaymentRepository.GetGuaranteePaymentById(id);

            if (guaranteePayment == null)
            {
                _logger.LogError("Guarantee Payment not found");
                throw new Exception("Guarantee Payment not found");
            }

            var guaranteePaymentObject = _mapper.Map<Domain.Dto.Query.GuaranteePaymentDto>(guaranteePayment);
            guaranteePaymentObject.GuaranteePaymentStatusName = guaranteePayment.GuaranteePaymentStatus_Name;
            guaranteePaymentObject.EventProviderNameProvider = guaranteePayment.EventProvider_NameProvider;

            return guaranteePaymentObject;
        }

        /// <summary>
        /// Actualiza la información de una garantía de pago existente.
        /// </summary>
        /// <param name="id">Identificador de la garantía de pago.</param>
        /// <param name="requestguaranteepayment">Datos actualizados de la garantía de pago.</param>
        /// <returns>DTO actualizado de la garantía de pago.</returns>
        /// <exception cref="Exception">Si no se encuentra la garantía de pago.</exception>
        public async Task<GuaranteePaymentDto> UpdatedGuaranteePaymentAsync(int id, GuaranteePaymentDto requestguaranteepayment)
        {
            var guaranteePaymentObject = await _unitOfWork.GuaranteePaymentRepository.GetByIdAsync(id);
            if (guaranteePaymentObject == null)
            {
                _logger.LogError("Guarantee Payment not found");
                throw new Exception("Guarantee Payment not found");
            }
            var guaranteePayment = _mapper.Map<GuaranteePayment>(requestguaranteepayment);
            guaranteePayment.Id = id;
            guaranteePayment.UpdatedAt = DateTime.Now;
            guaranteePayment.CreatedAt = guaranteePaymentObject.CreatedAt;
            guaranteePayment.GuaranteePaymentStatusId = guaranteePaymentObject.GuaranteePaymentStatusId;
            var eventProviderUpdate = await _unitOfWork.GuaranteePaymentRepository.UpdateAsync(guaranteePayment);
            await _unitOfWork.CompleteAsync();
            var guaranteePaymentResult = _mapper.Map<GuaranteePaymentDto>(eventProviderUpdate);

            var eventObject = await _unitOfWork.GuaranteePaymentRepository.GetEventLogProjectionByGuaranteeIdAsync(id);
            var EventCode = eventObject?.ClientCode + "-" + eventObject?.Id.ToString();

            var description = string.Format(Constans.MESSAGE_GUARANTEE_PAYMENT_UPDATE, eventObject!.Id, eventObject.VoucherName);
            await _auditLogService.SendEventLogAsync(new RequestLogData
            {
                EventId = eventObject!.Id,
                Action = EnumActions.GUARANTEE_PAYMENT_UPDATE.ToString(),
                Description = description,
                EventCode = EventCode,
                Role = Constans.MessageRole,
                SourceId = eventObject?.ClientId.ToString(),
                SourceCode = eventObject?.ClientCode,
                StatusEvent = eventObject?.EventStatusName,
                StatusEventId = eventObject?.EventStatusId.ToString(),
                UserName = Constans.MessageUserName
            });

            return guaranteePaymentResult;
        }
    }
}
