using Es_Log.Models.Entities;
using Es_Log.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Es_Log.Services
{
    public class Repository<T>(ESLogDbContext context) : IRepository<T>
       where T : class
    {
        private readonly ESLogDbContext _context = context;
        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            if (expression is not null)
                return await query.AnyAsync(expression);

            if (expression != null)
                query = query.Where(expression);

            return await query.AnyAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression = null, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            if (expression is not null)
                return await query.CountAsync(expression);

            if (expression != null)
                query = query.Where(expression);

            return await query.CountAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, bool tracking = true, params Expression<Func<T, object>>[] includeEntity)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            if (includeEntity.Any())
                foreach (var include in includeEntity)
                    query = query.Include(include);

            if (expression != null)
                query = query.Where(expression);

            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null, bool tracking = true, params Expression<Func<T, object>>[] includeEntity)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            if (includeEntity.Any())
                foreach (var include in includeEntity)
                    query = query.Include(include);

            if (expression != null)
                query = query.Where(expression);

            return await query.SingleOrDefaultAsync();
        }

        public async Task<T> GetByAsync(int id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            return await Table.FindAsync(id);
        }

        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> entity)
        {
            await Table.AddRangeAsync(entity);
            return true;
        }

        public bool Delete(T entity)
        {
            EntityEntry entityEntry = Table.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            T entity = await Table.FindAsync(id);
            return Delete(entity);
        }

        public bool DeleteRange(List<T> entity)
        {
            Table.RemoveRange(entity);
            return true;
        }

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

        public bool Update(T entity)
        {
            EntityEntry entityEntry = Table.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
