using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio específico para la entidad ViewEvent.
    /// Hereda de Repository<ViewEvent> y expone operaciones de acceso a datos para la vista de eventos.
    /// </summary>
    public class ViewEventsRepository : Repository<ViewEvent>, IViewEventsRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase ViewEventsRepository con el contexto de base de datos proporcionado.
        /// </summary>
        /// <param name="context">Contexto principal de la base de datos.</param>
        public ViewEventsRepository(MainContext context) : base(context)
        {
        }
    }
}
