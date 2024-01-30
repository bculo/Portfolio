namespace Stock.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    public IStockRepository StockRepo { get; }
    public IStockPriceRepository StockPriceRepo { get; }
    public IStockWithPriceTagReadRepository StockWithPriceTag { get; }

    Task Save(CancellationToken cls);
}