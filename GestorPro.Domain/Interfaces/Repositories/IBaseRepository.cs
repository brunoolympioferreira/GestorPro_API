using GestorPro.Domain.Interfaces.Contracts;
using System.Linq.Expressions;

namespace GestorPro.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : IBaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> GetByIdAsyncNoTracking(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}
