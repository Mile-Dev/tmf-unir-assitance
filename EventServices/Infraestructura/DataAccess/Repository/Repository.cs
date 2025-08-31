using EventServices.Domain.Dto;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedServices.ExpressionHelper;
using SharedServices.Objects;
using System.Linq.Expressions;

namespace EventServices.Infraestructura.DataAccess.Repository
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
        }
        public async Task<T> UpdateAsync(T entity)
        {
            var key = _context.Model.FindEntityType(typeof(T))!
                     .FindPrimaryKey()!
                     .Properties[0];

            var id = key.PropertyInfo!.GetValue(entity);
            var existing = await _context.Set<T>().FindAsync(id) ?? throw new Exception("Entities not found");
            _context.Entry(existing).CurrentValues.SetValues(entity);

            return existing;
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
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

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<PaginatedData<T>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Set<T>()
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .AsNoTracking();

            var data = await query.ToListAsync(cancellationToken);
            var totalCount = await _context.Set<T>().CountAsync(cancellationToken);

            return new PaginatedData<T>(data, totalCount);
        }

        public async Task<PaginatedData<T>> GetPaginatedData(int pageNumber, int pageSize, List<ExpressionFilter> filters, CancellationToken cancellationToken)
        {
            var query = _context.Set<T>().AsNoTracking();

            // Apply search criteria if provided
            if (filters != null && filters.Count != 0)
            {
                var expressionTree = ExpressionBuilder.ConstructAndExpressionTree<T>(filters);
                query = query.Where(expressionTree);
            }

            // Pagination
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalCount = await query.CountAsync(cancellationToken);

            return new PaginatedData<T>(data, totalCount);
        }

        public async Task<PaginatedData<T>> GetPaginatedData(int pageNumber, int pageSize, List<ExpressionFilter> filters, string sortBy, string sortOrder, CancellationToken cancellationToken)
        {
            var query = Entities.AsNoTracking();

            // Apply search criteria if provided
            if (filters != null && filters.Count != 0)
            {
                var expressionTree = ExpressionBuilder.ConstructAndExpressionTree<T>(filters);
                Console.WriteLine(expressionTree);
                query = query.Where(expressionTree);
            }

            // Add sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                var orderByExpression = GetOrderByExpression<T>(sortBy);
                query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(orderByExpression) : query.OrderBy(orderByExpression);
            }

            // Pagination
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalCount = await query.CountAsync(cancellationToken);

            return new PaginatedData<T>(data, totalCount);
        }

        public async Task<PaginatedData<T>> GetPaginatedData(List<Expression<Func<T, object>>> includeExpressions, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Set<T>()
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .AsQueryable();

            if (includeExpressions != null)
            {
                query = includeExpressions.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
            }

            var data = await query.AsNoTracking().ToListAsync(cancellationToken);
            var totalCount = await _context.Set<T>().CountAsync(cancellationToken);

            return new PaginatedData<T>(data, totalCount);
        }

        private Expression<Func<T, object>> GetOrderByExpression<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var conversion = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(conversion, parameter);
        }
    }
}
