using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Application.Modules.Crypto.Queries.FetchPage;
using Crypto.Core.Exceptions;
using Crypto.Core.ReadModels;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQueryHandler : IRequestHandler<FetchPriceHistoryQuery, List<PriceHistoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        private readonly IFusionCache _cache;

        public FetchPriceHistoryQueryHandler(IMapper mapper, 
            IUnitOfWork work, 
            IFusionCache cache)
        {
            _mapper = mapper;
            _work = work;
            _cache = cache;
        }

        public async Task<List<PriceHistoryDto>> Handle(FetchPriceHistoryQuery request, CancellationToken ct)
        {
            var items = await _cache.GetOrSetAsync(CacheKeys.FetchPriceHistoryKey(request),
                async (token) =>
                {
                    var items = await _work.CryptoPriceRepo.GetTimeFrameData(request.CryptoId, 
                        new TimeFrameQuery(43200, 30), 
                        ct);

                    return _mapper.Map<List<PriceHistoryDto>>(items);
                },
                CacheKeys.FetchPriceHistoryKeyOptions(),
                ct);

            return items;
        }
    }
}
