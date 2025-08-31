using EventServices.Domain.Entities;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using EventServices.Infraestructura.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventServices.Infraestructura.DataAccess.Dao
{
    /// <summary>
    /// Repositorio para la gestión de entidades <see cref="Category"/>.
    /// Implementa operaciones específicas para la entidad categoría.
    /// </summary>
    /// <remarks>
    /// Inicializa una nueva instancia de <see cref="CategoriesRepository"/>.
    /// </remarks>
    /// <param name="context">Contexto de base de datos principal.</param>
    public class CategoriesRepository(MainContext context) : Repository<Category>(context), ICategoriesRepository
    {

        /// <summary>
        /// Verifica si existe una categoría con el nombre especificado.
        /// </summary>
        /// <param name="name">Nombre de la categoría a buscar.</param>
        /// <returns>
        /// <c>true</c> si existe una categoría con ese nombre; en caso contrario, <c>false</c>.
        /// </returns>
        public async Task<bool> GetByName(string name)
            => await Entities.AnyAsync(x => x.Name == name);
    }
}
