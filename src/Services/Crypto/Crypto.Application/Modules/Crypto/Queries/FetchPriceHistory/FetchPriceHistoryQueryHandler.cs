using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQueryHandler(
        IMapper mapper,
        IUnitOfWork work,
        IFusionCache cache)
        : IRequestHandler<FetchPriceHistoryQuery, List<PriceHistoryDto>>
    {
        public async Task<List<PriceHistoryDto>> Handle(FetchPriceHistoryQuery request, CancellationToken ct)
        {
            var items = await cache.GetOrSetAsync(CacheKeys.FetchPriceHistoryKey(request),
                async (token) =>
                {
                    var items = await work.CryptoPriceRepo.GetTimeFrameData(request.CryptoId, 
                        new TimeFrameQuery(43200, 30), 
                        ct);

                    return mapper.Map<List<PriceHistoryDto>>(items);
                },
                CacheKeys.FetchPriceHistoryKeyOptions(),
                ct);

            return items;
        }
    }
}
