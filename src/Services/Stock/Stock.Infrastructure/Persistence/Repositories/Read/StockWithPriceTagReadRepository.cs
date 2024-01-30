using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;

namespace Stock.Infrastructure.Persistence.Repositories.Read;

public class StockWithPriceTagReadRepository : 
    BaseReadRepository<StockWithPriceTagReadModel>, IStockWithPriceTagReadRepository
{
    public StockWithPriceTagReadRepository(StockDbContext context) : base(context)
    {
    }
}