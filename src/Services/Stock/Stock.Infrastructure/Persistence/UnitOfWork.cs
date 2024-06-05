using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Interfaces.Repositories;

namespace Stock.Infrastructure.Persistence;

public class UnitOfWork(IServiceProvider provider, StockDbContext context) : IUnitOfWork
{
    public IStockRepository StockRepo { get; } = provider.GetRequiredService<IStockRepository>();
    public IStockPriceRepository StockPriceRepo { get; } = provider.GetRequiredService<IStockPriceRepository>();
    public IStockWithPriceTagReadRepository StockWithPriceTag { get; } = 
        provider.GetRequiredService<IStockWithPriceTagReadRepository>();


    public async Task Save(CancellationToken cls)
    {
        await context.SaveChangesAsync(cls);
    }
}