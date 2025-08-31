using EventServices.Domain.Dto;
using SharedServices.Objects;
using System.Linq.Expressions;

namespace EventServices.Infraestructura.DataAccess.Interface
{
    /// <summary>
    /// Define una interfaz genérica para operaciones de acceso a datos sobre entidades de tipo <typeparamref name="T"/>.
    /// Proporciona métodos para CRUD, paginación, filtrado y borrado lógico/físico.
    /// </summary>
    /// <typeparam name="T">Tipo de la entidad sobre la que se realizan las operaciones.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtiene una entidad por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>Entidad encontrada o null si no existe.</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las entidades, opcionalmente aplicando una lista de filtros.
        /// </summary>
        /// <param name="filters">Lista de expresiones de filtro opcionales.</param>
        /// <returns>Consulta de entidades filtradas.</returns>
        IQueryable<T> GetAll(List<Expression<Func<T, bool>>>? filters = null);

        /// <summary>
        /// Agrega una nueva entidad de forma asíncrona.
        /// </summary>
        /// <param name="entity">Entidad a agregar.</param>
        /// <returns>Entidad agregada.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad de forma asíncrona.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        /// <returns>Entidad actualizada.</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Actualiza una entidad de forma sincrónica.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
        void Update(T entity);

        /// <summary>
        /// Elimina una entidad de forma sincrónica.
        /// </summary>
        /// <param name="entity">Entidad a eliminar.</param>
        void Remove(T entity);

        /// <summary>
        /// Agrega un rango de entidades de forma asíncrona.
        /// </summary>
        /// <param name="entities">Colección de entidades a agregar.</param>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Actualiza un rango de entidades de forma asíncrona.
        /// </summary>
        /// <param name="entities">Colección de entidades a actualizar.</param>
        Task UpdateRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Elimina un rango de entidades de forma asíncrona.
        /// </summary>
        /// <param name="entities">Colección de entidades a eliminar.</param>
        Task DeleteRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Realiza un borrado lógico de una entidad por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        Task<bool> SoftDelete(int id);

        /// <summary>
        /// Realiza un borrado físico de una entidad por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>True si la operación fue exitosa, false en caso contrario.</returns>
        Task<bool> HardDelete(int id);

        /// <summary>
        /// Obtiene datos paginados de la entidad.
        /// </summary>
        /// <param name="pageNumber">Número de página.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Datos paginados.</returns>
        Task<PaginatedData<T>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken);

        /// <summary>
        /// Obtiene datos paginados aplicando filtros.
        /// </summary>
        /// <param name="pageNumber">Número de página.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <param name="filters">Lista de filtros a aplicar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Datos paginados filtrados.</returns>
        Task<PaginatedData<T>> GetPaginatedData(int pageNumber, int pageSize, List<ExpressionFilter> filters, CancellationToken cancellationToken);

        /// <summary>
        /// Obtiene datos paginados aplicando filtros y ordenamiento.
        /// </summary>
        /// <param name="pageNumber">Número de página.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <param name="filters">Lista de filtros a aplicar.</param>
        /// <param name="sortBy">Propiedad por la que ordenar.</param>
        /// <param name="sortOrder">Orden de la propiedad (ascendente o descendente).</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Datos paginados filtrados y ordenados.</returns>
        Task<PaginatedData<T>> GetPaginatedData(int pageNumber, int pageSize, List<ExpressionFilter> filters, string sortBy, string sortOrder, CancellationToken cancellationToken);

        /// <summary>
        /// Obtiene datos paginados incluyendo expresiones de navegación.
        /// </summary>
        /// <param name="includeExpressions">Expresiones de inclusión de propiedades de navegación.</param>
        /// <param name="pageNumber">Número de página.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Datos paginados con inclusiones.</returns>
        Task<PaginatedData<T>> GetPaginatedData(List<Expression<Func<T, object>>> includeExpressions, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
