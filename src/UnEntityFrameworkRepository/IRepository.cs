using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace UnEntityFrameworkRepository;

public interface IRepository<T> where T : class
{
    Task<IQueryable<T>> GetAsQueryable();
    Task<T[]> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null!);

    Task<T[]> GetAll(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null!);

    Task<T?> Get(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null!);

    Task Add(T entity);
    Task AddRange(T[] entities);
    Task Update(T entity);
    Task Remove(T entity);
    Task RemoveByPredicate(Expression<Func<T, bool>> predicate);
    Task RemoveRange(T[] entities);
}