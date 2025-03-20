using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Extensions;
using Crypto.Application.Common.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries;

public record GetMostPopularQuery : TakeQuery, IRequest<List<GetMostPopularResponse>>;

public record GetMostPopularResponse(string Symbol, int Count)
{
    public static GetMostPopularResponse Mapper(KeyValuePair<string, int> pair) => new(pair.Key, pair.Value);
}

public class GetMostPopularQueryValidator : TakeValidator<GetMostPopularQuery>;

public class GetMostPopularQueryHandler(
    IDateSourceProvider dataSourceProvider,
    IFusionCache cache)
    : IRequestHandler<GetMostPopularQuery, List<GetMostPopularResponse>>
{
    public async Task<List<GetMostPopularResponse>> Handle(GetMostPopularQuery request, CancellationToken ct)
    {
        var items = await cache.GetOrSetAsync(CacheKeys.MostPopularKey(request.Take),
            async (token) => await GetPopularItems(request.Take, token),
            CacheKeys.MostPopularKeyOptions(),
            ct);

        return items;
    }

    private async Task<List<GetMostPopularResponse>> GetPopularItems(int take, CancellationToken ct)
    {
        var result = await dataSourceProvider.GetForEntity<VisitEntity>()
            .Include(x => x.Crypto)
            .GroupBy(i => i.Crypto.Symbol)
            .OrderByDescending(i => i.Count())
            .Take(take)
            .ToDictionaryAsync(x => x.Key, x => x.Count(), ct);
            
        return result.Count == 0
            ? []
            : result.MapTo(GetMostPopularResponse.Mapper);
    }
}

