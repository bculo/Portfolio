using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Base;

namespace Stock.Infrastructure.Persistence;

public class DataSourceProvider(StockDbContext context) : IDataSourceProvider
{
    public IQueryable<T> GetQuery<T>() where T : class
    {
        return context.Set<T>();
    }
}