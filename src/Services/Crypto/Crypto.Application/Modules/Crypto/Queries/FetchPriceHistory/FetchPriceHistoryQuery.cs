using Crypto.Application.Models.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQuery : PageBaseQuery, IRequest<IEnumerable<PriceHistoryDto>>
    {
        public string Symbol { get; set; }
    }
}
