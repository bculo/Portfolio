using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;

namespace Stock.Infrastructure.Persistence.Repositories
{
    public class StockRepository(StockDbContext dbContext) : BaseRepository<StockEntity>(dbContext), IStockRepository;
}
