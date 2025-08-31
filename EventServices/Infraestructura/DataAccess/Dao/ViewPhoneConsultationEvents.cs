using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la entidad ViewPhoneConsultationEvent.
    /// Hereda de Repository y expone operaciones de acceso a datos para la vista de consultas telefónicas de eventos.
    /// </summary>
    public class ViewPhoneConsultationEventsRepository : Repository<ViewPhoneConsultationEvent>, IViewPhoneConsultationEventsRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia del repositorio con el contexto de base de datos proporcionado.
        /// </summary>
        /// <param name="context">Contexto principal de la base de datos.</param>
        public ViewPhoneConsultationEventsRepository(MainContext context) : base(context)
        {
        }
    }
}
