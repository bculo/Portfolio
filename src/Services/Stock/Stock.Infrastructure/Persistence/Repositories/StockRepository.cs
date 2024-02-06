using Microsoft.EntityFrameworkCore;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;

namespace Stock.Infrastructure.Persistence.Repositories
{
    public class StockRepository : BaseRepository<StockEntity>, IStockRepository
    {
        public StockRepository(StockDbContext dbContext) : base(dbContext)
        {
        }
    }
}
