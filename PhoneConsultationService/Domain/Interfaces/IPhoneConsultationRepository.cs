using PhoneConsultationService.Domain.Entities;

namespace PhoneConsultationService.Domain.Interfaces;

public interface IPhoneConsultationRepository
{
    /// <summary>
    /// Crea una nueva consulta telefónica de forma asíncrona
    /// </summary>
    /// <param name="phoneConsultation">La entidad de consulta telefónica a crear</param>
    /// <returns>La consulta telefónica creada</returns>
    Task<PhoneConsultation> CreateAsync(PhoneConsultation phoneConsultation);

    /// <summary>
    /// Obtiene una consulta telefónica por ID de evento e ID de registro telefónico de forma asíncrona
    /// </summary>
    /// <param name="idEvent">El identificador del evento</param>
    /// <param name="idPhoneRecord">El identificador del registro telefónico</param>
    /// <returns>La consulta telefónica encontrada</returns>
    Task<PhoneConsultation> GetIdPhoneRecordByIdEventAsync(string idEvent, string idPhoneRecord);

    /// <summary>
    /// Obtiene una lista de consultas telefónicas filtradas por ID de evento, campo y valor de forma asíncrona
    /// </summary>
    /// <param name="idEvent">El identificador del evento</param>
    /// <param name="field">El campo por el cual filtrar</param>
    /// <param name="value">El valor del campo a buscar</param>
    /// <returns>Una colección de consultas telefónicas que coinciden con los criterios</returns>
    Task<IEnumerable<PhoneConsultation>> GetListByIdAsync(string idEvent, string field, string value);

    /// <summary>
    /// Actualiza múltiples adjuntos en lote de forma asíncrona
    /// </summary>
    /// <param name="attachment">Lista de adjuntos a actualizar</param>
    /// <returns>True si la operación fue exitosa, false en caso contrario</returns>
    Task<bool> BatchWriteUpdateAsync(List<Attachment> attachment);

    /// <summary>
    /// Obtiene una lista de adjuntos filtrados por ID de evento, campo y valor de forma asíncrona
    /// </summary>
    /// <param name="idEvent">El identificador del evento</param>
    /// <param name="field">El campo por el cual filtrar</param>
    /// <param name="value">El valor del campo a buscar</param>
    /// <returns>Una colección de adjuntos que coinciden con los criterios</returns>
    Task<IEnumerable<Attachment>> GetListAttachmentByIdAsync(string idEvent, string field, string value);
}