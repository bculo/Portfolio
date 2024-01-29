using Stock.Core.Entities;
using Stock.Core.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.Interfaces.Persistence
{
    public interface IStockRepository : IBaseRepository<Core.Entities.Stock> 
    {
        public Task<List<StockPriceTagQuery>> GetAllWithPrice();
        public Task<StockPriceTagQuery> GetCurrentPrice(string symbol);
    }
}
