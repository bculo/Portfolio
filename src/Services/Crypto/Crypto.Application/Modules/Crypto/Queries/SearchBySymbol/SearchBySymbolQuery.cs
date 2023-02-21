using Crypto.Application.Models.Common;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Queries.SearchBySymbol
{
    public class SearchBySymbolQuery : PageBaseQuery, IRequest<SearchBySymbolResponse>
    {
        public string? Symbol { get; set; }
    }
}
