using Trend.Domain.Entities;

namespace Trend.Application.Interfaces;

public interface IMongoAuditableRepository<TEntity> : IRepository<TEntity> where TEntity : AuditableDocument
{
    Task<List<TEntity>> GetActiveItems(CancellationToken token);
    IAsyncEnumerable<TEntity> GetActiveItemsEnumerable(CancellationToken token);
    Task ActivateItems(IEnumerable<string> itemIds, CancellationToken token);
    Task DeactivateItems(IEnumerable<string> itemIds, CancellationToken token);
}