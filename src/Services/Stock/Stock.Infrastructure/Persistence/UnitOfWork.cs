using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Interfaces.Repositories;

namespace Stock.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly StockDbContext _context;
    
    public IStockRepository StockRepo { get; }
    public IStockPriceRepository StockPriceRepo { get; }
    
    public IStockWithPriceTagReadRepository StockWithPriceTag { get; }

    
    public UnitOfWork(IServiceProvider provider, StockDbContext context)
    {
        _context = context;

        StockRepo = provider.GetRequiredService<IStockRepository>();
        StockPriceRepo = provider.GetRequiredService<IStockPriceRepository>();
        StockWithPriceTag = provider.GetRequiredService<IStockWithPriceTagReadRepository>();
    }
    
    public async Task Save(CancellationToken cls)
    {
        await _context.SaveChangesAsync(cls);
    }
}