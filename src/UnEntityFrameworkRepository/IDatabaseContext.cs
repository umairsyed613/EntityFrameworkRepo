using Microsoft.EntityFrameworkCore;

namespace UnEntityFrameworkRepository;

public interface IDatabaseContext
{
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}