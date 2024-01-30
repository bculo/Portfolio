using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.Common.Models
{
    public class StockCacheItem
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get;set; }
    }
}
