using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.Interfaces
{
    public interface IStockPriceClient
    {
        Task<StockPriceInfo> GetPriceForSymbol(string symbol);
    }

    public class StockPriceInfo
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime FetchedTimestamp { get; set; }
    }
}
