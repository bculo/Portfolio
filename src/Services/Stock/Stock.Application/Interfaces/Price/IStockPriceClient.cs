using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.Application.Interfaces.Price.Models;

namespace Stock.Application.Interfaces.Price
{
    public interface IStockPriceClient
    {
        Task<StockPriceInfo?> GetPrice(string symbol, CancellationToken ct = default);
    }
}
