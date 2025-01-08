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
    public class FetchSingleQueryHandler(
        IMapper mapper,
        IUnitOfWork work,
        IPublishEndpoint publish,
        IFusionCache cache)
        : IRequestHandler<FetchSingleQuery, FetchSingleResponseDto>
    {
        public async Task<FetchSingleResponseDto> Handle(FetchSingleQuery request, CancellationToken ct)
        {
            var item = await cache.GetOrSetAsync(CacheKeys.SingleItemKey(request.Symbol),
                async (token) =>
                {
                    var instance = await work.CryptoPriceRepo.GetLastPrice(request.Symbol, token);
                    return instance is null ? null : mapper.Map<FetchSingleResponseDto>(instance);
                },
                CacheKeys.SingleItemKeyOptions(),
                ct);

            if (item == null)
            {
                throw new CryptoCoreNotFoundException($"Item with symbol {request.Symbol} not found");
            }
            
            await publish.Publish(new Visited
            {
                CryptoId = item.Id,
                Symbol = item.Symbol
            }, ct);
            
            return item;
        }
    }
}
