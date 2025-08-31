using SharedServices.Objects;
using System.Linq.Expressions;
using TrackingMokServices.Domain.Dto;

namespace TrackingMokServices.Infraestructura.DataAccess.Interface
{
    public interface IRepository<T> where T : class 
    {
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAll(List<Expression<Func<T, bool>>>? filters = null);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<bool> SoftDelete(int id);
        Task<bool> HardDelete(int id);
        Task<PaginatedData<T>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<PaginatedData<T>> GetPaginatedData(int pageNumber, int pageSize, List<ExpressionFilter> filters, CancellationToken cancellationToken);
        Task<PaginatedData<T>> GetPaginatedData(int pageNumber, int pageSize, List<ExpressionFilter> filters, string sortBy, string sortOrder, CancellationToken cancellationToken);
        Task<PaginatedData<T>> GetPaginatedData(List<Expression<Func<T, object>>> includeExpressions, int pageNumber, int pageSize, CancellationToken cancellationToken);

    }
}
