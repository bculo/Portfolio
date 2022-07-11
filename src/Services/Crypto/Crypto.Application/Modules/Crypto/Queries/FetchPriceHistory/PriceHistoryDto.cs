using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class PriceHistoryDto
    {
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
    }
}
