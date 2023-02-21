using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Application.Modules.Crypto.Queries.SearchBySymbol
{
    public class SearchBySymbolQueryHandler : IRequestHandler<SearchBySymbolQuery, SearchBySymbolResponse>
    {
        private readonly ILogger<SearchBySymbolQueryHandler> _logger;

        public SearchBySymbolQueryHandler(ILogger<SearchBySymbolQueryHandler> logger)
        {
            _logger = logger;
        }

        public Task<SearchBySymbolResponse> Handle(SearchBySymbolQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
