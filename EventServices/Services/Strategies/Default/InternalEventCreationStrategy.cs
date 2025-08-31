using AutoMapper;
using EventServices.Common;
using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Dto.Query;
using EventServices.Domain.Entities;
using EventServices.Infraestructura.AuditLog;
using EventServices.Infraestructura.DataAccess.Interface;
using SharedServices.Objects;

namespace EventServices.Services.Strategies.Default
{
    /// <summary>
    /// Estrategia concreta para la creación, obtención y actualización de eventos internos
    /// para el cliente Terrawind. Implementa la lógica específica para la gestión de eventos
    /// asociados a este cliente, incluyendo validaciones, persistencia y mapeo de datos.
    /// Hereda de <see cref="BaseEventCreationStrategy"/>.
    /// </summary>
    public class InternalEventCreationStrategy(
                   IUnitOfWork unitOfWork,
                   IMapper mapper,
                   ILogger<BaseEventCreationStrategy> logger,
                   IAuditLogService auditLogService) : BaseEventCreationStrategy(unitOfWork, mapper, logger, auditLogService)
    {
        /// <summary>
        /// Obtiene la clave del cliente asociada a esta estrategia.
        /// </summary>
        public override string ClientKey => Constans.ClientTerrawind;

        /// <summary>
        /// Crea un nuevo evento para el cliente Terrawind.
        /// Realiza validaciones, crea o asocia vouchers y customer trips, actualiza información de contacto,
        /// crea el evento y sus coberturas, y retorna un DTO con el resultado de la creación.
        /// </summary>
        /// <param name="input">Datos de entrada para la creación del evento.</param>
        /// <returns>DTO con el identificador y código del evento creado.</returns>
        /// <exception cref="InvalidOperationException">Si el cliente no existe o la validación de CustomerTrip falla.</exception>
        /// <exception cref="ArgumentException">Si la identificación del CustomerTrip es nula o vacía.</exception>
        public override async Task<ResponseCreatedDto> CreateEventAsync(RequestEvent input)
        {
            var clientId = input.Client ?? int.Parse(Constans.ClientTerrawind);
            var objectClient = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
            if (objectClient is null)
            {
                _logger.LogError("Client with id '{objectClient}' was not found.", Constans.ClientTerrawind);
                throw new InvalidOperationException($"Client with id '{Constans.ClientTerrawind}' was not found.");
            }

            if (string.IsNullOrWhiteSpace(input.EventCustomerTrip?.IdentificationCustomerTrip))
            {
                _logger.LogError("IdentificationCustomerTrip es obligatorio para el cliente default.");
                throw new ArgumentException("IdentificationCustomerTrip es obligatorio.");
            }

            var resultVouchers = await GetOrCreateVouchersAsync(input, objectClient.Id);
            var resultCustomerTrip = await GetOrCreateCustomerTripAsync(input) ?? throw new InvalidOperationException("CustomerTrip validation failed.");
            await _unitOfWork.CompleteAsync();

            await UpdatedContactInformationAsync(input, resultCustomerTrip);
            var resultEvent = await CreateObjectEventAsync(input, resultVouchers, resultCustomerTrip);

            ResponseCreatedDto responseCreateEventDto = new()
            {
                Id = resultEvent.Id,
                CodeEvent = objectClient.Code + "-" + resultEvent.Id.ToString()
            };

            var description = string.Format(Constans.MESSAGE_EVENT_CREATE, resultEvent.Id, resultEvent.Voucher_Name);
            await SendMessageAsync(new RequestLogData
            {
                EventId = resultEvent.Id,
                Action = EnumActions.EVENT_CREATE.ToString(),
                Description = description,
                EventCode = responseCreateEventDto.CodeEvent,
                Role = Constans.MessageRole,
                SourceId = objectClient.Id.ToString(),
                SourceCode = objectClient.Code,
                StatusEvent = resultEvent.EventStatus_Name,
                StatusEventId = resultEvent.EventStatusId.ToString(),
                UserName = Constans.MessageUserName,
                SendToClient = !Constans.MESSAGE_SEND
            });

            await GetOrCreateContactEmergencyAsync(input, resultEvent);
            await CreateEventCoveragesAsync(input, resultEvent);

           
            return responseCreateEventDto;
        }

        /// <summary>
        /// Obtiene los detalles de un evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>DTO con los detalles del evento.</returns>
        /// <exception cref="Exception">Propaga cualquier excepción ocurrida durante la obtención.</exception>
        public override async Task<ViewEventDetailsGetDto> GetEventAsync(int id)
        {
            try
            {
                var vieweventdetails = await _unitOfWork.ViewEventDetailsRepository.GetByIdAsync(id);
                var getAllviewevents = _mapper.Map<ViewEventDetailsGetDto>(vieweventdetails);
                return getAllviewevents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while geting the vieweventDetails: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los detalles de un evento por su código.
        /// </summary>
        /// <param name="codeEvent">Código del evento.</param>
        /// <returns>DTO con los detalles del evento.</returns>
        /// <exception cref="Exception">Propaga cualquier excepción ocurrida durante la obtención.</exception>
        public override async Task<ViewEventDetailsGetDto> GetEventByCodeAsync(string codeEvent)
        {
            try
            {
                var vieweventdetails = await _unitOfWork.ViewEventDetailsRepository.GetByCode(codeEvent);
                var viewevents = _mapper.Map<ViewEventDetailsGetDto>(vieweventdetails);
                return viewevents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while geting the GetEventDetailsByCode: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Obtiene una lista de eventos asociados a un voucher.
        /// </summary>
        /// <param name="voucher">Código del voucher.</param>
        /// <returns>Lista de DTOs con los detalles de los eventos.</returns>
        /// <exception cref="Exception">Propaga cualquier excepción ocurrida durante la obtención.</exception>
        public override async Task<List<ViewEventDetailsGetDto>> GetEventByVoucherAsync(string voucher)
        {
            try
            {
                var eventdetails = await _unitOfWork.ViewEventDetailsRepository.GetByVoucher(voucher);
                var listeventdetails = _mapper.Map<List<ViewEventDetailsGetDto>>(eventdetails);
                return listeventdetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while geting the GetListEventDetailsByVoucher: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de un evento existente, así como su voucher y customer trip asociados.
        /// </summary>
        /// <param name="id">Identificador del evento a actualizar.</param>
        /// <param name="input">Datos de entrada para la actualización del evento.</param>
        /// <returns>DTO con la información de la actualización.</returns>
        /// <exception cref="KeyNotFoundException">Si el evento, voucher o customer trip no existen.</exception>
        /// <exception cref="Exception">Propaga cualquier excepción ocurrida durante la actualización.</exception>
        public override async Task<ResponseUpdatedDto> UpdateEventAsync(int id, RequestUpdatedEvent input)
        {
            try
            {
                var eventDetail = await _unitOfWork.EventsRepository.GetByIdAsync(id);

                if (eventDetail == null)
                {
                    _logger.LogError("Event with ID '{Id}' was not found.", id);
                    throw new KeyNotFoundException($"Event with ID '{id}' was not found.");
                }

                eventDetail.EventStatusId = input.EventStatusId != 0 ? input.EventStatusId : eventDetail.EventStatusId;
                eventDetail.GeneralTypesId = input.TypeAssistanceIdEvent != 0 ? input.TypeAssistanceIdEvent : eventDetail.GeneralTypesId;
                eventDetail.Description = input.Description ?? eventDetail.Description;
                eventDetail.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.EventsRepository.UpdateAsync(eventDetail);

                var voucher = await _unitOfWork.VouchersRepository.GetByIdAsync(eventDetail.VoucherId);
                if (voucher == null)
                {
                    _logger.LogError("Voucher with ID '{VoucherId}' was not found.", eventDetail.VoucherId);
                    throw new KeyNotFoundException($"Voucher with ID '{eventDetail.VoucherId}' was not found.");
                }
                voucher.VoucherStatusId = input.VoucherStatusId != 0 ? input.VoucherStatusId : voucher.VoucherStatusId;
                voucher.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.VouchersRepository.UpdateAsync(voucher);

                Client? objectClient = await _unitOfWork.ClientRepository.GetByIdAsync(voucher.ClientId);
                EventStatus? eventStatus = await _unitOfWork.EventStatusRepository.GetByIdAsync(eventDetail.EventStatusId);

                var customerTrip = await _unitOfWork.CustomerTripRepository.GetByIdAsync(eventDetail.CustomerTripId);

                if (customerTrip == null)
                {
                    _logger.LogError("CustomerTrip with ID '{CustomerTripId}' was not found.", eventDetail.CustomerTripId);
                    throw new KeyNotFoundException($"CustomerTrip with ID '{eventDetail.CustomerTripId}' was not found.");
                }

                customerTrip.Gender = input.GenderCustomerTrip >= 0 && input.GenderCustomerTrip < 2 ? input.GenderCustomerTrip : customerTrip.Gender;
                customerTrip.DateOfBirth = input.BirthDateCustomerTrip != null ? DateTime.Parse(input.BirthDateCustomerTrip) : customerTrip.DateOfBirth;
                customerTrip.Names = input.NameCustomerTrip ?? customerTrip.Names;
                customerTrip.LastNames = input.LastNameCustomerTrip ?? customerTrip.LastNames;
                await _unitOfWork.CustomerTripRepository.UpdateAsync(customerTrip);
                await _unitOfWork.CompleteAsync();

                RequestEvent requestEvent = new()
                {
                    EventCustomerTrip = new RequestEventCustomerTripDto
                    {
                        IdentificationCustomerTrip = input.IdentificationCustomerTrip,
                        PhoneCustomerTrip = input.PhoneCustomerTrip,
                        EmailCustomerTrip = input.EmailCustomerTrip,
                    },
                };

                await UpdatedContactInformationAsync(requestEvent, customerTrip);

                var EventCode = objectClient?.Code + "-" + eventDetail.Id.ToString();
                var description = string.Format(Constans.MESSAGE_EVENT_UPDATE, eventDetail.Id, voucher.Name);
                await SendMessageAsync(new RequestLogData
                {
                    EventId = eventDetail.Id,
                    Action = EnumActions.EVENT_UPDATE.ToString(),
                    Description = description,
                    EventCode = EventCode,
                    Role = Constans.MessageRole,
                    SourceId = objectClient?.Id.ToString(),
                    SourceCode = objectClient?.Code,
                    StatusEvent = eventStatus?.Name,
                    StatusEventId = eventDetail.EventStatusId.ToString(),
                    UserName = Constans.MessageUserName,
                    SendToClient = !Constans.MESSAGE_SEND
                });

                return new ResponseUpdatedDto
                {
                    Id = id,
                    UpdateAt = DateTime.UtcNow,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the event: {Message}", ex.Message);
                throw;
            }
        }
    }
}
