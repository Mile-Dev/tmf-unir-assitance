using EventStatusSwitchTempServices.Infraestructura.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace EventStatusSwitchTempServices.Infraestructura.DataAccess.Repository
{
    public abstract class Repository<T>(MainContext context) : IRepository<T> where T : class
    {

        protected readonly MainContext _context = context;
        protected DbSet<T> Entities => _context.Set<T>();

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();

        }

        public bool Update(T entity, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                _context.Set<T>().Update(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            EntityEntry<T> insertedValue = await _context.Set<T>().AddAsync(entity);
            return insertedValue.Entity;
        }

        public IQueryable<T> GetAll(List<Expression<Func<T, bool>>>? filters = null)
        {
            IQueryable<T> query = Entities;

            // Si filters es null o no contiene elementos, no aplicar ningún filtro
            if (filters != null && filters.Count != 0)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            return query;
        }

        public async Task<bool> SoftDelete(int id)
        {
            T? entity = await GetByIdAsync(id);

            if (entity is null)
                return false;

            //entity.IsDeleted = true;
            //entity.DeletedTimeUtc = DateTime.UtcNow;

            _context.Set<T>().Update(entity);
            return true;
        }

        public async Task<bool> HardDelete(int id)
        {
            T? entity = await GetByIdAsync(id);
            if (entity is null)
                return false;
            _context.Set<T>().Remove(entity);
            return true;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities);
        }

        public async Task<(IQueryable<T> Data, int TotalCount)> GetFilteredDataAsync(
            List<Expression<Func<T, bool>>> filters,
            int pageNumber,
            int pageSize)
        {
            // Aplicar filtros
            IQueryable<T> query = Entities;
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }

            // Obtener el conteo total antes de la paginación
            int totalCount = await query.CountAsync();

            // Aplicar paginación
            var paginatedData = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return (paginatedData, totalCount);
        }
    }
}
