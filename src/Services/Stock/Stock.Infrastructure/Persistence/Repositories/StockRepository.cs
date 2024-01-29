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

        public async Task<List<StockWithPriceTagReadModel>> GetAllWithPrice()
        {
            return await _dbContext.StockWithPriceTag.ToListAsync();
        }

        public async Task<StockWithPriceTagReadModel> GetCurrentPrice(string symbol)
        {
            return await _dbContext.StockWithPriceTag.Where(i => i.Symbol == symbol).FirstOrDefaultAsync();
        }
    }
}
