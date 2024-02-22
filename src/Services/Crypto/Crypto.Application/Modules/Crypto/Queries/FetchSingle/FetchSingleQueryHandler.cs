using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries.FetchSingle
{
    public class FetchSingleQueryHandler : IRequestHandler<FetchSingleQuery, FetchSingleResponseDto>
    {
        private readonly IFusionCache _cache;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work; 
        private readonly IPublishEndpoint _publish;

        public FetchSingleQueryHandler(IMapper mapper, 
            IUnitOfWork work, 
            IPublishEndpoint publish, IFusionCache cache)
        {
            _mapper = mapper;
            _work = work;
            _publish = publish;
            _cache = cache;
        }

        public async Task<FetchSingleResponseDto> Handle(FetchSingleQuery request, CancellationToken ct)
        {
            var item = await _cache.GetOrSetAsync(CacheKeys.CryptoItemKey(request.Symbol),
                async (token) =>
                {
                    var instance = await _work.CryptoPriceRepo.GetLastPrice(request.Symbol, token);
                    return instance is null ? null : _mapper.Map<FetchSingleResponseDto>(instance);
                },
                CacheKeys.CryptoItemKeyOptions(),
                ct);
            
            CryptoCoreException.ThrowIfNull(item, $"Item with symbol {request.Symbol} not found");
            
            await _publish.Publish(new Visited
            {
                CryptoId = item.Id,
                Symbol = item.Symbol
            }, ct);
            
            return item;
        }
    }
}
