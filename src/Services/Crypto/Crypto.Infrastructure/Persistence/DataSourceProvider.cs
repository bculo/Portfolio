using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;

namespace Crypto.Infrastructure.Persistence;

public class DataSourceProvider(CryptoDbContext dbContext) : IDateSourceProvider
{
    public IQueryable<T> GetForEntity<T>() where T : Entity
    {
        return dbContext.Set<T>();
    }
}