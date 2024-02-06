using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;

namespace Stock.Infrastructure.Persistence.Repositories;

public class StockPriceRepository : BaseRepository<StockPriceEntity>, IStockPriceRepository
{
    public StockPriceRepository(StockDbContext dbContext) : base(dbContext)
    {
    }
}