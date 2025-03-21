using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using Crypto.Infrastructure.Persistence;

namespace Crypto.IntegrationTests;


public class TestFixture(CryptoDbContext context, IUnitOfWork cryptoPriceRepo, IServiceProvider provider)
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

    public async Task<CryptoPriceEntity> AddPrice(CryptoPriceEntity price)
    {
        await cryptoPriceRepo.CryptoPriceRepo.Add(price);
        return price;
    }
}