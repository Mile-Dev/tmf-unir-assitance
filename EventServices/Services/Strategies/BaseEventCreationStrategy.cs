using AutoMapper;
using EventServices.Common;
using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Dto.Query;
using EventServices.Domain.Entities;
using EventServices.Infraestructura.AuditLog;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;

/// <summary> 
namespace EventServices.Services.Strategies
{
    /// <summary>
    /// Clase base abstracta para la estrategia de creación de eventos.
    /// Proporciona métodos y lógica común para la gestión de eventos, incluyendo la creación, actualización,
    /// obtención y validación de entidades relacionadas como Voucher, CustomerTrip, ContactInformation, ContactEmergency y EventCoverage.
    /// Implementa la interfaz <see cref="IEventCreationStrategy"/> y debe ser extendida por estrategias concretas para clientes específicos.
    /// </summary>
    public abstract class BaseEventCreationStrategy(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<BaseEventCreationStrategy> logger,
        IAuditLogService auditLogService) : IEventCreationStrategy
    {
        /// <summary>
        /// Unidad de trabajo para acceso a repositorios y persistencia de datos.
        /// </summary>
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Instancia de AutoMapper para mapeo de entidades y DTOs.
        /// </summary>
        protected readonly IMapper _mapper = mapper;

        /// <summary>
        /// Logger para registro de información y errores.
        /// </summary>
        protected readonly ILogger<BaseEventCreationStrategy> _logger = logger;

        /// <summary>
        /// Servicio para envío de mensajes a SQS.
        /// </summary>
        private readonly IAuditLogService _auditLogService = auditLogService;

        /// <summary>
        /// Clave del cliente asociada a la estrategia concreta.
        /// </summary>
        public abstract string ClientKey { get; }

        /// <summary>
        /// Lógica principal para crear un evento. Debe ser implementada por cada estrategia concreta.
        /// </summary>
        /// <param name="input">Datos de entrada para la creación del evento.</param>
        /// <returns>DTO con la respuesta de creación.</returns>
        public abstract Task<ResponseCreatedDto> CreateEventAsync(RequestEvent input);

        /// <summary>
        /// Lógica principal para editar un evento. Debe ser implementada por cada estrategia concreta.
        /// </summary>
        /// <param name="id">Identificador del evento a actualizar.</param>
        /// <param name="input">Datos de entrada para la actualización del evento.</param>
        /// <returns>DTO con la respuesta de actualización.</returns>
        public abstract Task<ResponseUpdatedDto> UpdateEventAsync(int id, RequestUpdatedEvent input);

        /// <summary>
        /// Obtiene un evento por ID. Debe ser implementada por cada estrategia concreta.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>DTO con los detalles del evento.</returns>
        public abstract Task<ViewEventDetailsGetDto> GetEventAsync(int id);

        /// <summary>
        /// Obtiene un evento por código. Debe ser implementada por cada estrategia concreta.
        /// </summary>
        /// <param name="codeEvent">Código del evento.</param>
        /// <returns>DTO con los detalles del evento.</returns>
        public abstract Task<ViewEventDetailsGetDto> GetEventByCodeAsync(string codeEvent);

        /// <summary>
        /// Obtiene una lista de eventos por voucher. Debe ser implementada por cada estrategia concreta.
        /// </summary>
        /// <param name="voucher">Código del voucher.</param>
        /// <returns>Lista de DTOs con los detalles de los eventos.</returns>
        public abstract Task<List<ViewEventDetailsGetDto>> GetEventByVoucherAsync(string voucher);

        /// <summary>
        /// Valida la existencia de un voucher en la base de datos o lo crea si no existe.
        /// </summary>
        /// <param name="responsefirscontact">Datos del evento.</param>
        /// <param name="clientid">Identificador del cliente.</param>
        /// <returns>Entidad <see cref="Voucher"/> existente o creada.</returns>
        protected async Task<Voucher> GetOrCreateVouchersAsync(RequestEvent responsefirscontact, int clientid)
        {
            var voucher = await _unitOfWork.VouchersRepository.GetByNameAsync(responsefirscontact.EventVoucher.NameVoucher, clientid);
            if (voucher == null)
            {
                voucher = _mapper.Map<Voucher>(responsefirscontact.EventVoucher);
                voucher.ClientId = clientid;
                voucher = await _unitOfWork.VouchersRepository.AddAsync(voucher);
            }
            return voucher;
        }

        /// <summary>
        /// Valida la existencia de un CustomerTrip en la base de datos o lo crea si no existe.
        /// </summary>
        /// <param name="responsefirscontact">Datos del evento.</param>
        /// <returns>Entidad <see cref="CustomerTrip"/> existente o creada, o null si no se puede crear.</returns>
        protected async Task<CustomerTrip?> GetOrCreateCustomerTripAsync(RequestEvent responsefirscontact)
        {
            var contactInformation = await _unitOfWork.ContactInformationRepository.GetByValue(responsefirscontact.EventCustomerTrip.IdentificationCustomerTrip, Convert.ToInt32(responsefirscontact.EventCustomerTrip.TypeIdentificationCustomerTrip));
            if (contactInformation == null)
            {
                CustomerTrip? customerTripResult = _mapper.Map<CustomerTrip>(responsefirscontact.EventCustomerTrip);
                customerTripResult = await _unitOfWork.CustomerTripRepository.AddAsync(customerTripResult);
                return customerTripResult;
            }

            return contactInformation.CustomerTrip;
        }

        /// <summary>
        /// Actualiza o inserta la información de contacto asociada a un CustomerTrip.
        /// </summary>
        /// <param name="responseFirstContact">Datos del evento.</param>
        /// <param name="resultCustomerTrip">Entidad CustomerTrip asociada.</param>
        protected async Task UpdatedContactInformationAsync(RequestEvent responseFirstContact, CustomerTrip resultCustomerTrip)
        {
            var potencialContacts = new List<ContactInformation?>()
            {
                CreateContactInformation(resultCustomerTrip.Id, Convert.ToInt32(responseFirstContact.EventCustomerTrip.TypeIdentificationCustomerTrip), responseFirstContact.EventCustomerTrip.IdentificationCustomerTrip),
                CreateContactInformation(resultCustomerTrip.Id, 5, responseFirstContact.EventCustomerTrip.PhoneCustomerTrip),
                CreateContactInformation(resultCustomerTrip.Id, 3, responseFirstContact.EventCustomerTrip.EmailCustomerTrip)
            };

            Expression<Func<ContactInformation, bool>>? filter = e => e.CustomerTripId == resultCustomerTrip.Id;
            List<Expression<Func<ContactInformation, bool>>> expressions = [filter];
            var contactsInformation = await _unitOfWork.ContactInformationRepository.GetAll(expressions).ToListAsync();

            if (contactsInformation.Count != 0)
            {
                var updated = false;
                foreach (var existing in contactsInformation)
                {
                    var matching = potencialContacts.FirstOrDefault(p => p!.GeneralTypesId == existing.GeneralTypesId);
                    if (matching != null && matching.Value != existing.Value)
                    {
                        existing.Value = matching.Value;
                        updated = true;
                    }
                }
                if (updated)
                {
                    await _unitOfWork.ContactInformationRepository.UpdateRangeAsync(contactsInformation);
                }
            }
            else
            {
                var toInsert = await BuildContactInformationListAsync(resultCustomerTrip, potencialContacts);
                if (toInsert.Count != 0)
                {
                    await _unitOfWork.ContactInformationRepository.AddRangeAsync(toInsert);
                    _logger.LogInformation("Se insertaron nuevos contactos: {@toInsert}", toInsert);
                }
            }
            await _unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// Construye una lista de objetos <see cref="ContactInformation"/> que aún no existen en la base de datos
        /// para un <see cref="CustomerTrip"/> dado, a partir de una lista potencial de contactos.
        /// Filtra los contactos nulos y verifica cuáles valores ya existen en la base de datos para evitar duplicados.
        /// </summary>
        /// <param name="resultCustomerTrip">Entidad <see cref="CustomerTrip"/> asociada a los contactos.</param>
        /// <param name="contactInformationPotencialList">Lista potencial de contactos (puede contener valores nulos).</param>
        /// <returns>Lista de <see cref="ContactInformation"/> que no existen en la base de datos y deben ser insertados.</returns>
        protected async Task<List<ContactInformation>> BuildContactInformationListAsync(CustomerTrip resultCustomerTrip, List<ContactInformation?> contactInformationPotencialList)
        {

            contactInformationPotencialList = [.. contactInformationPotencialList.Where(x => x != null)];

            var existingValues = await _unitOfWork.ContactInformationRepository
                                       .ExistValue(resultCustomerTrip.Id, [.. contactInformationPotencialList.Select(c => c!.GeneralTypesId)]);

            var contactInformationList = contactInformationPotencialList.Where(contact => !existingValues.Contains(contact!.Value)).Cast<ContactInformation>().ToList();

            return contactInformationList;
        }

        /// <summary>
        /// Crea y guarda un nuevo evento en la base de datos a partir de los datos proporcionados.
        /// </summary>
        /// <param name="responseFirstContact">Datos de entrada del evento.</param>
        /// <param name="resultVouchers">Entidad voucher asociada al evento.</param>
        /// <param name="resultCustomerTrip">Entidad customer trip asociada al evento.</param>
        /// <returns>Entidad <see cref="Event"/> creada y persistida.</returns>
        protected async Task<Event> CreateObjectEventAsync(RequestEvent responseFirstContact, Voucher resultVouchers, CustomerTrip resultCustomerTrip)
        {
            try
            {
                var eventsObject = MapObjectEvent(responseFirstContact, resultVouchers, resultCustomerTrip);
                var resultEvent = await _unitOfWork.EventsRepository.AddAsync(eventsObject);
                await _unitOfWork.CompleteAsync();
                return resultEvent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el evento");
                throw;
            }
        }

        /// <summary>
        /// Crea o asocia contactos de emergencia a un evento y los guarda en la base de datos.
        /// </summary>
        /// <param name="responseFirstContact">Datos de entrada del evento.</param>
        /// <param name="resultEvent">Entidad <see cref="Event"/> asociada.</param>
        /// <returns>Tarea asincrónica.</returns>
        protected async Task GetOrCreateContactEmergencyAsync(RequestEvent responseFirstContact, Event resultEvent)
        {
            var resultContactEmergency = _mapper.Map<List<ContactEmergency>>(responseFirstContact.EventEmergencyContact.ListEmergencyContactEvent);
            resultContactEmergency.ForEach(x => x.EventId = resultEvent.Id);
            await _unitOfWork.ContactEmergencyRepository.AddRangeAsync(resultContactEmergency);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("resultContactEmergency: {@resultContactEmergency}", resultContactEmergency);
        }

        /// <summary>
        /// Crea y guarda la cobertura del evento en la base de datos.
        /// </summary>
        /// <param name="responseFirstContact">Datos de entrada del evento.</param>
        /// <param name="resultEvent">Entidad <see cref="Event"/> asociada.</param>
        /// <returns>Tarea asincrónica.</returns>
        protected async Task CreateEventCoveragesAsync(RequestEvent responseFirstContact, Event resultEvent)
        {
            EventCoveragesDto eventCoveragesDto = new()
            {
                EventId = resultEvent.Id,
                CoverageVoucher = responseFirstContact.EventDetails.CoverageEvent,
            };

            var resultEventCoverages = _mapper.Map<EventCoverage>(eventCoveragesDto);
            await _unitOfWork.EventCoveragesRepository.AddAsync(resultEventCoverages);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("CreateObjectEventCoverages: {@resultEventCoverages}", resultEventCoverages);
        }

        /// <summary>
        /// Crea una instancia de <see cref="ContactInformation"/> si el valor proporcionado no es nulo ni vacío.
        /// </summary>
        /// <param name="customerTripId">Identificador del viaje del cliente.</param>
        /// <param name="generalTypesId">Identificador del tipo general de contacto.</param>
        /// <param name="value">Valor del contacto.</param>
        /// <returns>Instancia de <see cref="ContactInformation"/> o null si el valor es vacío.</returns>
        private static ContactInformation? CreateContactInformation(int customerTripId, int generalTypesId, string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return new ContactInformation
            {
                CustomerTripId = customerTripId,
                GeneralTypesId = generalTypesId,
                Value = value
            };
        }

        /// <summary>
        /// Mapea los datos de entrada y entidades relacionadas a una nueva instancia de <see cref="Event"/>.
        /// </summary>
        /// <param name="requestEvent">Datos de entrada del evento.</param>
        /// <param name="resultVouchers">Entidad voucher asociada.</param>
        /// <param name="resultCustomerTrip">Entidad customer trip asociada.</param>
        /// <returns>Instancia de <see cref="Event"/> mapeada.</returns>
        protected virtual Event MapObjectEvent(RequestEvent requestEvent, Voucher resultVouchers, CustomerTrip resultCustomerTrip)
        {
            var eventsObject = _mapper.Map<Event>(requestEvent.EventDetails);
            eventsObject.CustomerTripId = resultCustomerTrip.Id;
            eventsObject.VoucherId = resultVouchers.Id;
            eventsObject.EventStatusId = 2;
            eventsObject.GeneralTypesId = requestEvent.EventDetails.TypeAssistanceIdEvent;
            eventsObject.CountryLocation = requestEvent.EventLocation.CountryEventLocation;
            eventsObject.CityLocation = requestEvent.EventLocation.CityEventLocation;
            eventsObject.AddressLocation = requestEvent.EventLocation.AddressEventLocation;
            eventsObject.InformationLocation = requestEvent.EventLocation.InformationLocation;
            var gpsValues = requestEvent.EventLocation.GpsEventLocation?.Split(',');

            if (gpsValues != null && gpsValues.Length > 1)
            {
                _ = double.TryParse(gpsValues[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude);
                _ = double.TryParse(gpsValues[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude);

                eventsObject.Longitude = longitude;
                eventsObject.Latitude = latitude;
            }

            return eventsObject;
        }

        /// <summary>
        /// Envía un mensaje a la cola SQS configurada.
        /// </summary>
        /// <param name="requestLogData">Datos a enviar.</param>
        /// <returns>Tarea asincrónica.</returns>
        protected virtual async Task SendMessageAsync(RequestLogData requestLogData)
        {
            await _auditLogService.SendEventLogAsync(requestLogData);
        }

        /// <summary>
        /// Crea y guarda la información del proveedor de evento en la base de datos.
        /// </summary>
        /// <param name="responseEventProvider">DTO con los datos del proveedor del evento.</param>
        /// <returns>Tarea asincrónica.</returns>
        protected async Task CreateEventProviderAsync(EventProvider eventprovider)
        {
            await _unitOfWork.EventProviderRepository.AddAsync(eventprovider);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("CreateObjectEventProvider from  the event: {@resultEventProvider}", eventprovider);
        }

        /// <summary>
        /// Mapea los datos de entrada de un evento y la entidad Event asociada para crear una nueva instancia de <see cref="EventProvider"/>.
        /// Obtiene el identificador del estado "No Provider Assigned" desde el repositorio de estados de proveedores.
        /// Construye el objeto EventProvider con los datos relevantes, incluyendo el identificador del evento, el estado, el identificador externo de la solicitud,
        /// los datos externos serializados y el subtipo de asistencia. Asigna las fechas de creación y actualización al momento actual.
        /// </summary>
        /// <param name="requestEvent">Datos de entrada del evento, incluyendo información adicional para el proveedor.</param>
        /// <param name="eventid">Entidad <see cref="Event"/> asociada al proveedor.</param>
        /// <returns>Instancia de <see cref="EventProvider"/> mapeada y lista para ser persistida.</returns>
        protected virtual async Task<EventProvider> MapObjectEventProviderAsync(RequestEvent requestEvent, Event eventid)
        {
            var statusIdNoProviderAssigned = await _unitOfWork.EventProviderStatusRepository.GetStatusIdByCodeAsync(Constans.STATUS_EP_NO_PROVIDER_ASSIGNED);

            return new()
            {
                EventId = eventid.Id,
                EventProviderStatusId = statusIdNoProviderAssigned,
                ExternalRequestId = string.Concat(requestEvent?.FieldsAditionalsMok?.CaseId, Constans.SeparatorExternalRequest, requestEvent?.FieldsAditionalsMok?.IdOt),
                ExternalData = JsonSerializer.Serialize(requestEvent?.FieldsAditionalsMok),
                AssistanceSubTypeId = eventid.EventStatusId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }
    }
}
