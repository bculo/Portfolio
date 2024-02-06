using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;

namespace Stock.Infrastructure.Persistence.Repositories;

public class StockPriceIterationRepository : BaseRepository<StockPriceIterationEntity>, IStockPriceIterationRepository
{
    public StockPriceIterationRepository(StockDbContext dbContext) : base(dbContext)
    {
    }
}