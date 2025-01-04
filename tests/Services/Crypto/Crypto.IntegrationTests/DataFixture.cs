using AutoFixture;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using Crypto.Infrastructure.Persistence;
using Tests.Common.Extensions;

namespace Crypto.IntegrationTests;

public class DataFixture(CryptoDbContext context)
{
    public async Task<T> Add<T>(T entity) where T : Entity
    {
        context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    
    public async Task AddRange<T>(List<T> entities) where T : Entity
    {
        context.Set<T>().AddRange(entities);
        await context.SaveChangesAsync();
    }
}