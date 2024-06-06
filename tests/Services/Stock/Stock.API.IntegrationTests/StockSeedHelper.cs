using Sqids;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;
using Stock.Infrastructure.Persistence;

namespace Stock.API.IntegrationTests;

public class StockSeedHelper(
    StockDbContext context,
    SqidsEncoder<int> encoder) : IDisposable
{
    public async Task<StockEntity> Create(string symbol)
    {
        var entity = new StockEntity
        {
            Symbol = symbol,
            IsActive = true
        };

        await context.Stocks.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    
    public async Task<(StockEntity entity, string id)> CreateWithEncodedId(string symbol)
    {
        var entity = await Create(symbol);
        return (entity, encoder.Encode(entity.Id));
    }
    
    public void Dispose()
    {
        context?.Dispose();
    }
}