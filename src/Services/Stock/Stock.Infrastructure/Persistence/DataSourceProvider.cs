using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Base;

namespace Stock.Infrastructure.Persistence;

public class DataSourceProvider(StockDbContext context) : IDataSourceProvider
{
    public IQueryable<T> GetQuery<T>() where T : AuditableEntity => context.Set<T>();
    public IQueryable<T> GetReadOnlySourceQuery<T>() where T : ReadOnlySource => context.Set<T>();
}