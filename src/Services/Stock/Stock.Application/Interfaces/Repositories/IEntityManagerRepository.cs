using Stock.Core.Models.Base;

namespace Stock.Application.Interfaces.Repositories;

public interface IEntityManagerRepository
{
    Task Add<T>(T entity, CancellationToken ct = default) where T : Entity;
    Task AddRange<T>(IEnumerable<T> entities, CancellationToken ct = default) where T : Entity;
    Task Delete<T>(T entity, CancellationToken ct = default) where T : Entity;
    Task DeleteById<T>(Guid id, CancellationToken ct = default) where T : Entity;
    Task DeleteByIds<T>(IEnumerable<Guid> ids, CancellationToken ct = default) where T : Entity;
    Task SaveChanges(CancellationToken ct = default);
}