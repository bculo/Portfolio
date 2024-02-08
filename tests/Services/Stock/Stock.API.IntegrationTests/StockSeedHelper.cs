using Sqids;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;
using Stock.Infrastructure.Persistence;

namespace Stock.API.IntegrationTests;

public class StockSeedHelper : IDisposable
{
    private readonly StockDbContext _context;
    private readonly SqidsEncoder<int> _encoder;
    
    public StockSeedHelper(StockDbContext context, 
        SqidsEncoder<int> encoder)
    {
        _context = context;
        _encoder = encoder;
    }

    public async Task<StockEntity> Create(string symbol)
    {
        var entity = new StockEntity
        {
            Symbol = symbol,
            IsActive = true
        };

        await _context.Stocks.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
    public async Task<(StockEntity entity, string id)> CreateWithEncodedId(string symbol)
    {
        var entity = await Create(symbol);
        return (entity, _encoder.Encode(entity.Id));
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }
}