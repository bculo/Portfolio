using AutoMapper;
using Crypto.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crypto.Application.Modules.Crypto.Queries.SearchBySymbol
{
    public class SearchBySymbolQueryHandler : IRequestHandler<SearchBySymbolQuery, List<SearchBySymbolResponse>>
    {
        private readonly ILogger<SearchBySymbolQueryHandler> _logger;
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public SearchBySymbolQueryHandler(ILogger<SearchBySymbolQueryHandler> logger,
            IUnitOfWork work,
            IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<SearchBySymbolResponse>> Handle(SearchBySymbolQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // var cryptos = await _work.CryptoRepository.SearchBySymbol(request.Symbol, request.Page, request.Take);
            //
            // return _mapper.Map<List<SearchBySymbolResponse>>(cryptos);
        }
    }
}
