using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Interfaces.Repositories;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries.GetMostPopular
{
    public class GetMostPopularQueryHandler(
        IUnitOfWork work,
        IMapper mapper,
        IFusionCache cache)
        : IRequestHandler<GetMostPopularQuery, List<GetMostPopularResponse>>
    {
        public async Task<List<GetMostPopularResponse>> Handle(GetMostPopularQuery request, CancellationToken ct)
        {
            var items = await cache.GetOrSetAsync(CacheKeys.MostPopularKey(request.Take),
                async (token) =>
                {
                    var response = await work.VisitRepo.GetMostPopular(request.Take, token);
                    return response.Count == 0 
                        ? []
                        : mapper.Map<List<GetMostPopularResponse>>(response);
                },
                CacheKeys.MostPopularKeyOptions(),
                ct);

            return items ?? [];
        }
    }
}
