using EventServices.Domain.Dto;

namespace EventServices.Services.Interfaces
{
    /// <summary>
    /// Define los servicios para cambiar el estado de un evento.
    /// </summary>
    public interface IEventStatusChangeServices
    {
        /// <summary>
        /// Actualiza el estado de un evento específico.
        /// </summary>
        /// <param name="Id">Identificador del evento.</param>
        /// <param name="eventStatus">Objeto con la información del nuevo estado.</param>
        /// <returns>Respuesta con el estado actualizado del evento.</returns>
        Task<ResponseEventStatus> UpdatedStatusOnEventAsync(int Id, RequestEventStatus eventStatus);

        /// <summary>
        /// Marca un evento como completado.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        Task<bool> CompleteEventAsync(int id);

        /// <summary>
        /// Cancela un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        Task<bool> CancelEventAsync(int id);

        /// <summary>
        /// Re abrir un evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        Task<bool> ReOpenEventAsync(int id);
    }
}
