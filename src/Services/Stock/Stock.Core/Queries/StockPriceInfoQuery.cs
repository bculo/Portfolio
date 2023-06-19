﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Queries
{
    public class StockPriceInfoQuery
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
    }
}
