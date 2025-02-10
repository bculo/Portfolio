using Microsoft.EntityFrameworkCore;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Base;

namespace Stock.Infrastructure.Persistence;

public class EntityManagerRepository(StockDbContext context) : IEntityManagerRepository
{
    public async Task Add<T>(T entity, CancellationToken ct = default) where T : Entity
    {
        await context.Set<T>().AddAsync(entity, ct);
    }
    
    public async Task AddRange<T>(IEnumerable<T> entities, CancellationToken ct = default) where T : Entity
    {
        await context.Set<T>().AddRangeAsync(entities, ct);
    }

    public Task Delete<T>(T entity, CancellationToken ct = default) where T : Entity
    { 
        context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteById<T>(Guid id, CancellationToken ct = default) where T : Entity
    {
        await context.Set<T>().Where(x => x.Id == id).ExecuteDeleteAsync(ct);
    }
    
    public async Task DeleteByIds<T>(IEnumerable<Guid> ids, CancellationToken ct = default) where T : Entity
    {
        await context.Set<T>().Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync(ct);
    }

    public async Task SaveChanges(CancellationToken ct = default)
    {
        await context.SaveChangesAsync(ct);
    }
}