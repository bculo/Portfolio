using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.Core.Models.Base;

namespace Stock.Core.Models.Stock
{
    public class StockWithPriceTagReadModel : IReadModel
    {
        public int StockId { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
    }
}
