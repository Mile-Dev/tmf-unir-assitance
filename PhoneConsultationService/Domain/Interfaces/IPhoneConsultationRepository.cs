using PhoneConsultationService.Domain.Entities;

namespace PhoneConsultationService.Domain.Interfaces;

public interface IPhoneConsultationRepository
{
    /// <summary>
    /// Crea una nueva consulta telef�nica de forma as�ncrona
    /// </summary>
    /// <param name="phoneConsultation">La entidad de consulta telef�nica a crear</param>
    /// <returns>La consulta telef�nica creada</returns>
    Task<PhoneConsultation> CreateAsync(PhoneConsultation phoneConsultation);

    /// <summary>
    /// Obtiene una consulta telef�nica por ID de evento e ID de registro telef�nico de forma as�ncrona
    /// </summary>
    /// <param name="idEvent">El identificador del evento</param>
    /// <param name="idPhoneRecord">El identificador del registro telef�nico</param>
    /// <returns>La consulta telef�nica encontrada</returns>
    Task<PhoneConsultation> GetIdPhoneRecordByIdEventAsync(string idEvent, string idPhoneRecord);

    /// <summary>
    /// Obtiene una lista de consultas telef�nicas filtradas por ID de evento, campo y valor de forma as�ncrona
    /// </summary>
    /// <param name="idEvent">El identificador del evento</param>
    /// <param name="field">El campo por el cual filtrar</param>
    /// <param name="value">El valor del campo a buscar</param>
    /// <returns>Una colecci�n de consultas telef�nicas que coinciden con los criterios</returns>
    Task<IEnumerable<PhoneConsultation>> GetListByIdAsync(string idEvent, string field, string value);

    /// <summary>
    /// Actualiza m�ltiples adjuntos en lote de forma as�ncrona
    /// </summary>
    /// <param name="attachment">Lista de adjuntos a actualizar</param>
    /// <returns>True si la operaci�n fue exitosa, false en caso contrario</returns>
    Task<bool> BatchWriteUpdateAsync(List<Attachment> attachment);

    /// <summary>
    /// Obtiene una lista de adjuntos filtrados por ID de evento, campo y valor de forma as�ncrona
    /// </summary>
    /// <param name="idEvent">El identificador del evento</param>
    /// <param name="field">El campo por el cual filtrar</param>
    /// <param name="value">El valor del campo a buscar</param>
    /// <returns>Una colecci�n de adjuntos que coinciden con los criterios</returns>
    Task<IEnumerable<Attachment>> GetListAttachmentByIdAsync(string idEvent, string field, string value);
}