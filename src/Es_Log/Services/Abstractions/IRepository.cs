using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Es_Log.Services.Abstractions
{
    public interface IRepository<T>
        where T : class
    {
        DbSet<T> Table { get; }
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, bool tracking = true, params Expression<Func<T, object>>[] includeEntity);
        Task<T> GetAsync(Expression<Func<T, bool>> expression = null, bool tracking = true, params Expression<Func<T, object>>[] includeEntity);
        Task<T> GetByAsync(int id, bool tracking = true);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression, bool tracking = true);
        Task<int> CountAsync(Expression<Func<T, bool>> expression = null, bool tracking = true);
        Task<bool> AddAsync(T entity);
        Task<bool> AddRangeAsync(List<T> entity);
        bool Update(T entity);
        bool Delete(T entity);
        Task<bool> DeleteAsync(int id);
        bool DeleteRange(List<T> entity);
        Task<bool> SaveChangesAsync();
    }
}
