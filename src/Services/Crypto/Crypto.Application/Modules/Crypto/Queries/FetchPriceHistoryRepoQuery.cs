using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Extensions;
using Crypto.Application.Common.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.ReadModels;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries;

public record FetchPriceHistoryRepoQuery(Guid CryptoId) : PageQuery, IRequest<List<PriceHistoryDto>>;

public record PriceHistoryDto(
    Guid CryptoId,
    string Symbol,
    string Name,
    string Website,
    string SourceCode,
    DateTimeOffset TimeBucket,
    decimal AvgPrice,
    decimal MaxPrice,
    decimal MinPrice,
    decimal LastPrice
)
{
    public static PriceHistoryDto Mapper(CryptoTimeFrameReadModel m)
        => new(m.CryptoId, m.Symbol, m.Name, m.Website, m.SourceCode, m.TimeBucket, m.AvgPrice, m.MaxPrice, m.MinPrice,
            m.LastPrice);
}

public class FetchPriceHistoryQueryHandler(
    IUnitOfWork work,
    IFusionCache cache)
    : IRequestHandler<FetchPriceHistoryRepoQuery, List<PriceHistoryDto>>
{
    public async Task<List<PriceHistoryDto>> Handle(FetchPriceHistoryRepoQuery request, CancellationToken ct)
    {
        var items = await cache.GetOrSetAsync(CacheKeys.FetchPriceHistoryKey(request),
            async (token) => await GetPriceHistory(request, token),
            CacheKeys.FetchPriceHistoryKeyOptions(),
            ct);

        return items;
    }
    
    private async Task<List<PriceHistoryDto>> GetPriceHistory(FetchPriceHistoryRepoQuery request, CancellationToken ct)
    {
        var items = await work.CryptoPriceRepo.GetTimeFrameData(request.CryptoId, 
            new TimeFrameQuery(43200, 30), 
            ct);

        return items.MapTo(PriceHistoryDto.Mapper);
    }
}