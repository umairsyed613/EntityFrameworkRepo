using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace UnEntityFrameworkRepository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IDatabaseContext _context;
    private readonly DbSet<T> _entities;

    public Repository(IDatabaseContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<T[]> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null!)
    {
        var query = _entities.AsQueryable();
        if (include != null)
        {
            query = include(query);
        }

        return await query.ToArrayAsync();
    }


    public async Task<T[]> GetAll(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        var query = _entities.AsQueryable();
        if (include != null)
        {
            query = include(query);
        }

        return await query.Where(predicate).ToArrayAsync();
    }

    public async Task<T?> Get(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        var query = _entities.AsQueryable();
        if (include != null)
        {
            query = include(query);
        }

        return await query.Where(predicate).FirstOrDefaultAsync();
    }

    public async Task Add(T entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRange(T[] entities)
    {
        await _entities.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(T entity)
    {
        _entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveByPredicate(Expression<Func<T, bool>> predicate)
    {
        var entity = await _entities.Where(predicate).FirstOrDefaultAsync();
        if (entity is null) throw new InvalidOperationException("Cannot remove entity not found!");

        _entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRange(T[] entities)
    {
        _entities.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }
}