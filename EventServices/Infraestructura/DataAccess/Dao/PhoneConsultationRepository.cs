using EventServices.Common;
using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para operaciones de acceso a datos relacionadas con la entidad PhoneConsultation.
    /// Hereda de Repository y proporciona métodos específicos para PhoneConsultation.
    /// </summary>
    public class PhoneConsultationRepository(MainContext context) : Repository<PhoneConsultation>(context), IPhoneConsultationRepository
    {
        /// <summary>
        /// Verifica si existe un registro de consulta telefónica agendado para un evento específico
        /// que no haya sido cancelado y que no tenga una fecha de finalización programada.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <returns>True si existe al menos un registro agendado, false en caso contrario.</returns>
        public async Task<bool> ExistsRecordScheduledAsync(int eventId)
         => await Entities.AnyAsync(item => item.EventId == eventId && item.ScheduledEndAt == default && item.Status != EnumStatusPhoneConsultation.Canceled.ToString());

        /// <summary>
        /// Obtiene la lista de consultas telefónicas asociadas a un evento específico.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <returns>Lista de PhoneConsultation relacionadas con el evento.</returns>
        public async Task<List<PhoneConsultation>> ListPhoneConsultationAsync(int eventId)
        {
            return await Entities
                     .Where(item => item.EventId == eventId)
                     .ToListAsync();
        }
    }
}
