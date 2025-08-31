using EventServices.Domain.Dto.Create;
using EventServices.Domain.Dto.Query;

namespace EventServices.Services.Interfaces
{
    /// <summary>
    /// Define los servicios para la gestión de consultas telefónicas programadas.
    /// </summary>
    public interface IPhoneConsultationServices
    {
        /// <summary>
        /// Crea una nueva consulta telefónica programada para un evento específico.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <param name="phoneConsultation">Datos de la consulta telefónica.</param>
        /// <returns>Objeto con información de la creación.</returns>
        Task<ResponseCreatedDto> CreatedScheduledPhoneConsultationAsync(int eventId, PhoneConsultationDto phoneConsultation);

        /// <summary>
        /// Cierra una consulta telefónica programada.
        /// </summary>
        /// <param name="id">Identificador de la consulta telefónica.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        Task<bool> ClosedScheduledPhoneConsultationAsync(int id);

        /// <summary>
        /// Cancela una consulta telefónica programada.
        /// </summary>
        /// <param name="id">Identificador de la consulta telefónica.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        Task<bool> CanceledScheduledPhoneConsultationAsync(int id);

        /// <summary>
        /// Reprograma la fecha y hora de una consulta telefónica programada existente.
        /// </summary>
        /// <param name="id">Identificador de la consulta telefónica a reprogramar.</param>
        /// <param name="reschedulePhoneConsultation">Datos con la nueva fecha y hora de la consulta.</param>
        /// <returns>True si la reprogramación fue exitosa, false en caso contrario.</returns>
        Task<bool> ReScheduledPhoneConsultationAsync(int id, ReschedulePhoneConsultationDto reschedulePhoneConsultation);

        /// <summary>
        /// Obtiene la lista de consultas telefónicas asociadas a un evento.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <returns>Lista de consultas telefónicas.</returns>
        Task<List<PhoneConsultationQueryDto>> GetListPhoneConsultationsByEventAsync(int eventId);
    }
}
