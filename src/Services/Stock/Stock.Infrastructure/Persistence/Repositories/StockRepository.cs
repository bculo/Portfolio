using Stock.Application.Interfaces;
using Stock.Core.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Persistence.Repositories
{
    public class StockRepository : BaseRepository<Core.Entities.Stock>, IStockRepository
    {
        public StockRepository(StockDbContext dbContext) : base(dbContext)
        {
        }

        public Task<StockPriceInfoQuery> GetCurrentPrice(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
