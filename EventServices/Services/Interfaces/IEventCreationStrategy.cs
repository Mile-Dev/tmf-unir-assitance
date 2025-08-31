using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using EventServices.Domain.Dto.Query;

namespace EventServices.Services.Interfaces
{
    /// <summary>
    /// Estrategia para la creación y gestión de eventos según el cliente.
    /// </summary>
    public interface IEventCreationStrategy
    {
        /// <summary>
        /// Clave única que identifica al cliente asociado a la estrategia.
        /// </summary>
        string ClientKey { get; }

        /// <summary>
        /// Obtiene una lista de eventos asociados a un voucher específico.
        /// </summary>
        /// <param name="voucher">Código del voucher.</param>
        /// <returns>Lista de detalles de eventos.</returns>
        Task<List<ViewEventDetailsGetDto>> GetEventByVoucherAsync(string voucher);

        /// <summary>
        /// Obtiene los detalles de un evento a partir de su código.
        /// </summary>
        /// <param name="codeEvent">Código del evento.</param>
        /// <returns>Detalles del evento.</returns>
        Task<ViewEventDetailsGetDto> GetEventByCodeAsync(string codeEvent);

        /// <summary>
        /// Obtiene los detalles de un evento a partir de su identificador.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Detalles del evento.</returns>
        Task<ViewEventDetailsGetDto> GetEventAsync(int id);

        /// <summary>
        /// Crea un nuevo evento con la información proporcionada.
        /// </summary>
        /// <param name="input">Datos para la creación del evento.</param>
        /// <returns>Respuesta con los datos del evento creado.</returns>
        Task<ResponseCreatedDto> CreateEventAsync(RequestEvent input);

        /// <summary>
        /// Actualiza un evento existente con la información proporcionada.
        /// </summary>
        /// <param name="id">Identificador del evento a actualizar.</param>
        /// <param name="input">Datos para la actualización del evento.</param>
        /// <returns>Respuesta con los datos del evento actualizado.</returns>
        Task<ResponseUpdatedDto> UpdateEventAsync(int id, RequestUpdatedEvent input);
    }
}
