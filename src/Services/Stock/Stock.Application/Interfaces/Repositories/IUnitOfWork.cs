namespace Stock.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    public IStockRepository StockRepo { get; }
    public IStockPriceRepository StockPriceRepo { get; }

    Task Save(CancellationToken cls);
}