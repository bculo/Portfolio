using Trend.Domain.Entities;

namespace Trend.Application.Interfaces;

public interface IMongoAuditableRepository<TEntity> : IRepository<TEntity> where TEntity : AuditableDocument
{
    Task<List<TEntity>> GetActiveItems(CancellationToken token = default);
    Task<List<TEntity>> GetDeactivatedItems(CancellationToken token = default);
    IAsyncEnumerable<TEntity> GetActiveItemsEnumerable(CancellationToken token = default);
    IAsyncEnumerable<TEntity> GetDeactivatedItemsEnumerable(CancellationToken token = default);
    Task ActivateItems(IEnumerable<string> itemIds, CancellationToken token = default);
    Task DeactivateItems(IEnumerable<string> itemIds, CancellationToken token = default);
}