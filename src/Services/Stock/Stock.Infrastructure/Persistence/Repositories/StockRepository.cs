using Microsoft.EntityFrameworkCore;
using Stock.Application.Interfaces;
using Stock.Core.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.Application.Interfaces.Persistence;

namespace Stock.Infrastructure.Persistence.Repositories
{
    public class StockRepository : BaseRepository<Core.Entities.Stock>, IStockRepository
    {
        public StockRepository(StockDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<StockPriceTagQuery>> GetAllWithPrice()
        {
            return await _dbContext.StockWithPriceTag.ToListAsync();
        }

        public async Task<StockPriceTagQuery> GetCurrentPrice(string symbol)
        {
            return await _dbContext.StockWithPriceTag.Where(i => i.Symbol == symbol).FirstOrDefaultAsync();
        }
    }
}
