using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPage
{
    public class FetchPageQueryHandler : IRequestHandler<FetchPageQuery, PageBaseResult<FetchPageResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        private readonly IFusionCache _cache;

        public FetchPageQueryHandler(IMapper mapper, IUnitOfWork work, IFusionCache cache)
        {
            _mapper = mapper;
            _work = work;
            _cache = cache;
        }

        public async Task<PageBaseResult<FetchPageResponseDto>> Handle(FetchPageQuery request, CancellationToken ct)
        {
            var items = await _cache.GetOrSetAsync(CacheKeys.FetchCryptoPageKey(request),
                async (token) =>
                {
                    var query = _mapper.Map<CryptoPricePageQuery>(request);
                    var repoResult = await _work.CryptoPriceRepo.GetPage(query, token);
                    var dtoItems = _mapper.Map<List<FetchPageResponseDto>>(repoResult.Items);
                    return new PageBaseResult<FetchPageResponseDto>(repoResult.TotalCount, request.Page, dtoItems);
                },
                CacheKeys.FetchCryptoPageKeyOptions(),
                ct);

            return items;
        }
    }
}
