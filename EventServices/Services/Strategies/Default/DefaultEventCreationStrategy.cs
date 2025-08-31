using AutoMapper;
using EventServices.Common;
using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Dto.Query;
using EventServices.Infraestructura.AuditLog;
using EventServices.Infraestructura.DataAccess.Interface;
using SQSProducerServices.Common.Interfaces;

namespace EventServices.Services.Strategies.Default
{
    /// <summary>
    /// Estrategia por defecto para la creación y gestión de eventos.
    /// Esta clase se utiliza cuando el clientKey proporcionado no coincide con ninguna estrategia concreta registrada.
    /// Todos los métodos lanzan una excepción indicando que el clientKey no es reconocido.
    /// </summary>
    public class DefaultEventCreationStrategy(
                       IUnitOfWork unitOfWork,
                       IMapper mapper,
                       ILogger<BaseEventCreationStrategy> logger,
                       IAuditLogService _auditLogService) : BaseEventCreationStrategy(unitOfWork, mapper, logger, _auditLogService)
    {
        /// <summary>
        /// Clave del cliente asociada a la estrategia por defecto.
        /// </summary>
        public override string ClientKey => Constans.ClientDefault;

        /// <summary>
        /// Lanza una excepción indicando que el clientKey no es reconocido al intentar crear un evento.
        /// </summary>
        /// <param name="input">Datos de entrada para la creación del evento.</param>
        /// <returns>No retorna valor, siempre lanza excepción.</returns>
        /// <exception cref="InvalidOperationException">Siempre lanzada para indicar clientKey no reconocido.</exception>
        public override Task<ResponseCreatedDto> CreateEventAsync(RequestEvent input)
        {
            throw new InvalidOperationException("No se reconoce el clientKey proporcionado.");
        }

        /// <summary>
        /// Lanza una excepción indicando que el clientKey no es reconocido al intentar obtener un evento por ID.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>No retorna valor, siempre lanza excepción.</returns>
        /// <exception cref="InvalidOperationException">Siempre lanzada para indicar clientKey no reconocido.</exception>
        public override Task<ViewEventDetailsGetDto> GetEventAsync(int id)
        {
            throw new InvalidOperationException("No se reconoce el clientKey proporcionado.");
        }

        /// <summary>
        /// Lanza una excepción indicando que el clientKey no es reconocido al intentar obtener un evento por código.
        /// </summary>
        /// <param name="codeEvent">Código del evento.</param>
        /// <returns>No retorna valor, siempre lanza excepción.</returns>
        /// <exception cref="InvalidOperationException">Siempre lanzada para indicar clientKey no reconocido.</exception>
        public override Task<ViewEventDetailsGetDto> GetEventByCodeAsync(string codeEvent)
        {
            throw new InvalidOperationException("No se reconoce el clientKey proporcionado.");
        }

        /// <summary>
        /// Lanza una excepción indicando que el clientKey no es reconocido al intentar obtener eventos por voucher.
        /// </summary>
        /// <param name="voucher">Código del voucher.</param>
        /// <returns>No retorna valor, siempre lanza excepción.</returns>
        /// <exception cref="InvalidOperationException">Siempre lanzada para indicar clientKey no reconocido.</exception>
        public override Task<List<ViewEventDetailsGetDto>> GetEventByVoucherAsync(string voucher)
        {
            throw new InvalidOperationException("No se reconoce el clientKey proporcionado.");
        }

        /// <summary>
        /// Lanza una excepción indicando que el clientKey no es reconocido al intentar actualizar un evento.
        /// </summary>
        /// <param name="id">Identificador del evento a actualizar.</param>
        /// <param name="input">Datos de entrada para la actualización del evento.</param>
        /// <returns>No retorna valor, siempre lanza excepción.</returns>
        /// <exception cref="InvalidOperationException">Siempre lanzada para indicar clientKey no reconocido.</exception>
        public override Task<ResponseUpdatedDto> UpdateEventAsync(int id, RequestUpdatedEvent input)
        {
            throw new InvalidOperationException("No se reconoce el clientKey proporcionado.");
        }
    }
}
