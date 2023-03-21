using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Queries.Response
{
    public class CryptoMostPopularQuery
    {
        public string Symbol { get; set; }
        public long Counter { get; set; }
    }
}
