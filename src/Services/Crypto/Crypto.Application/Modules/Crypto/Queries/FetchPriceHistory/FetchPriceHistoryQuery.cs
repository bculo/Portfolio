using Crypto.Application.Common.Models;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQuery : PageBaseQuery, IRequest<List<PriceHistoryDto>>
    {
        public Guid CryptoId { get; set; }
    }
}
