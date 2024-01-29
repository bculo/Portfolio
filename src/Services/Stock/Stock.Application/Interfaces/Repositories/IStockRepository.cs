using Stock.Core.Models;
using Stock.Core.Models.Stock;

namespace Stock.Application.Interfaces.Repositories
{
    public interface IStockRepository : IBaseRepository<StockEntity> 
    {
        /*
        public Task<List<StockWithPriceTagReadModel>> GetAllWithPrice();
        public Task<StockWithPriceTagReadModel> GetCurrentPrice(string symbol);
        */
    }
}
