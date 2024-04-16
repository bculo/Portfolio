using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using Crypto.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries.GetMostPopular
{
    public class GetMostPopularQueryHandler : IRequestHandler<GetMostPopularQuery, List<GetMostPopularResponse>>
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly IFusionCache _cache;

        public GetMostPopularQueryHandler(IUnitOfWork work, 
            IMapper mapper, 
            IFusionCache cache)
        {
            _work = work;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<GetMostPopularResponse>> Handle(GetMostPopularQuery request, CancellationToken ct)
        {
            var items = await _cache.GetOrSetAsync(CacheKeys.MostPopularKey(request.Take),
                async (token) =>
                {
                    var response = await _work.VisitRepo.GetMostPopular(request.Take, token);
                    return response.Count == 0 
                        ? new List<GetMostPopularResponse>() 
                        : _mapper.Map<List<GetMostPopularResponse>>(response);
                },
                CacheKeys.MostPopularKeyOptions(),
                ct);

            return items;
        }
    }
}
