using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.GetMostPopular
{
    public class GetMostPopularResponse
    {
        public string Symbol { get; set; } = default!;
        public int Count { get; set; }
    }
}
