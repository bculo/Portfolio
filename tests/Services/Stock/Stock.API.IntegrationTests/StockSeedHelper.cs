using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;
using Stock.Infrastructure.Persistence;

namespace Stock.API.IntegrationTests;

public class StockSeedHelper : IDisposable
{
    private readonly StockDbContext _context;
    
    public StockSeedHelper(StockDbContext context)
    {
        _context = context;
    }

    public async Task<StockEntity> CreateNew(string symbol)
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
    
    public void Dispose()
    {
        _context?.Dispose();
    }
}