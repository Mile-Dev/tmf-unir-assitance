using System.Linq.Expressions;

namespace MasterRdsServices.Infraestructura.DataAccess.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAll(List<Expression<Func<T, bool>>>? filters = null);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<bool> SoftDelete(int id);
        Task<bool> HardDelete(int id);
        Task<(IQueryable<T> Data, int TotalCount)> GetFilteredDataAsync(List<Expression<Func<T, bool>>> filters, int pageNumber, int pageSize);

    }
}
