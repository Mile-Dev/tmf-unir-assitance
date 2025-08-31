using EventServices.Domain.Dto.Create;

namespace EventServices.Services.Interfaces
{
    /// <summary>
    /// Define los servicios relacionados con la gestión de proveedores de eventos.
    /// </summary>
    public interface IEventProviderServices
    {
        /// <summary>
        /// Crea un nuevo proveedor de evento.
        /// </summary>
        /// <param name="eventProviderDto">Datos del proveedor de evento a crear.</param>
        /// <returns>DTO con información del registro creado.</returns>
        Task<ResponseCreatedDto> CreatedEventProviderAsync(EventProviderDto eventProviderDto);

        /// <summary>
        /// Obtiene un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>DTO con la información del proveedor de evento.</returns>
        Task<Domain.Dto.Query.EventProviderDto> GetEventProviderByIdAsync(int id);

        /// <summary>
        /// Actualiza la información de un proveedor de evento.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="eventProviderDto">Datos actualizados del proveedor de evento.</param>
        /// <returns>DTO actualizado del proveedor de evento.</returns>
        Task<Domain.Dto.Query.EventProviderDto> UpdatedEventProviderAsync(int id, EventProviderDto eventProviderDto);

        /// <summary>
        /// Elimina un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>True si la eliminación fue exitosa, false en caso contrario.</returns>
        Task<bool> DeletedEventProviderByIdAsync(int id);

        /// <summary>
        /// Obtiene la lista de proveedores de evento asociados a un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Lista de DTOs de proveedores de evento.</returns>
        Task<List<Domain.Dto.Query.EventProviderDto>> GetEventProviderByEventIdAsync(int id);

        /// <summary>
        /// Cancela un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>True si la cancelación fue exitosa, false en caso contrario.</returns>
        Task<Domain.Dto.Query.EventProviderDto> CanceledEventProviderByIdAsync(int id);

        /// <summary>
        /// Marca como completado un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        Task<Domain.Dto.Query.EventProviderDto> CompletedEventProviderByIdAsync(int id);

        /// <summary>
        /// Reprograma la cita de un proveedor de evento.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="rescheduleEventProvider">Datos de la nueva programación.</param>
        /// <returns>True si la reprogramación fue exitosa, false en caso contrario.</returns>
        Task<Domain.Dto.Query.EventProviderDto> RescheduledEventProviderByIdAsync(int id, RescheduleEventProviderDto rescheduleEventProvider);

        /// <summary>
        /// Actualiza el diagnóstico de un proveedor de evento por su identificador.
        /// </summary>
        /// <param name="id">Identificador del proveedor de evento.</param>
        /// <param name="diagnostic">Nuevo diagnóstico a asignar.</param>
        /// <returns>DTO con la información actualizada del proveedor de evento.</returns>
        Task<Domain.Dto.Query.EventProviderDto> UpdateDiagnosticEventProviderByIdAsync(int id, RequestUpdateDiagnosticDto diagnostic);
    }
}
