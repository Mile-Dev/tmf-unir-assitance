using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la entidad GeneralType.
    /// Hereda de Repository<GeneralType> y expone operaciones específicas para GeneralType.
    /// </summary>
    public class GeneralTypesRepository : Repository<GeneralType>, IGeneralTypesRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de GeneralTypesRepository con el contexto de base de datos proporcionado.
        /// </summary>
        /// <param name="context">Contexto principal de la base de datos.</param>
        public GeneralTypesRepository(MainContext context) : base(context)
        {
        }
    }
}
