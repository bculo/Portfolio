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

        public async Task<List<StockWithPriceTag>> GetAllWithPrice()
        {
            return await Context.StockWithPriceTag.ToListAsync();
        }

        public async Task<StockWithPriceTag> GetCurrentPrice(string symbol)
        {
            return await Context.StockWithPriceTag.Where(i => i.Symbol == symbol).FirstOrDefaultAsync();
        }
    }
}
