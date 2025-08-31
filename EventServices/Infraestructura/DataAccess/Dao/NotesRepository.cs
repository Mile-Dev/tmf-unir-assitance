using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio específico para la entidad EventNote.
    /// Hereda de Repository<EventNote> y expone las operaciones definidas en INotesRepository.
    /// </summary>
    public class NotesRepository(MainContext context) : Repository<EventNote>(context), INotesRepository
    {
        // Actualmente no se agregan métodos adicionales.
        // Hereda los métodos CRUD básicos de Repository<EventNote>.
    }
}
