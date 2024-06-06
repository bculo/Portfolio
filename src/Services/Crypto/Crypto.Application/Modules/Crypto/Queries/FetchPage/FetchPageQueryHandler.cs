using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPage
{
    public class FetchPageQueryHandler(IMapper mapper, IUnitOfWork work, IFusionCache cache)
        : IRequestHandler<FetchPageQuery, PageBaseResult<FetchPageResponseDto>>
    {
        public async Task<PageBaseResult<FetchPageResponseDto>> Handle(FetchPageQuery request, CancellationToken ct)
        {
            var items = await cache.GetOrSetAsync(CacheKeys.FetchCryptoPageKey(request),
                async (token) =>
                {
                    var query = mapper.Map<CryptoPricePageQuery>(request);
                    var repoResult = await work.CryptoPriceRepo.GetPage(query, token);
                    var dtoItems = mapper.Map<List<FetchPageResponseDto>>(repoResult.Items);
                    return new PageBaseResult<FetchPageResponseDto>(repoResult.TotalCount, request.Page, dtoItems);
                },
                CacheKeys.FetchCryptoPageKeyOptions(),
                ct);

            return items;
        }
    }
}
