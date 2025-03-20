using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Extensions;
using Crypto.Application.Common.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.ReadModels;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries;

public record GetMostPopularQuery : TakeQuery, IRequest<List<GetMostPopularResponse>>;

public record GetMostPopularResponse(string Symbol, int Count)
{
    public static GetMostPopularResponse Mapper(MostPopularReadModel model) => new(model.Symbol, model.Count);
}

public class GetMostPopularQueryValidator : TakeValidator<GetMostPopularQuery>;

public class GetMostPopularQueryHandler(
    IUnitOfWork work,
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
        var response = await work.VisitRepo.GetMostPopular(take, ct);
        return response.Count == 0
            ? []
            : response.MapTo(GetMostPopularResponse.Mapper);
    }
}

