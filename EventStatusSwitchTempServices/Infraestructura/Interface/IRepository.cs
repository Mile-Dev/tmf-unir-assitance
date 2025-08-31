using System.Linq.Expressions;

namespace EventStatusSwitchTempServices.Infraestructura.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAll(List<Expression<Func<T, bool>>>? filters = null);
        Task<T> AddAsync(T entity);
        bool Update(T entity, out string errorMessage);
        void Remove(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<bool> SoftDelete(int id);
        Task<bool> HardDelete(int id);
        Task<(IQueryable<T> Data, int TotalCount)> GetFilteredDataAsync(List<Expression<Func<T, bool>>> filters, int pageNumber, int pageSize);
    }
}
