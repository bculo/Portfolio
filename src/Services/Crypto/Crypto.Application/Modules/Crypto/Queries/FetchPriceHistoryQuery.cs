using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.ReadModels;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries;

public record FetchPriceHistoryQuery(Guid CryptoId) : PageBaseQuery, IRequest<List<PriceHistoryDto>>;

public record PriceHistoryDto
{
    public Guid CryptoId { get; init; }
    public string Symbol { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Website { get; init; } = null!;
    public string SourceCode { get; init; } = null!;
    public DateTimeOffset TimeBucket { get; init; }
    public decimal AvgPrice { get; init; }
    public decimal MaxPrice { get; init; }
    public decimal MinPrice { get; init; }
    public decimal LastPrice { get; init; }
}

public class FetchPriceHistoryQueryMapper : Profile
{
    public FetchPriceHistoryQueryMapper()
    {
        CreateMap<CryptoTimeFrameReadModel, PriceHistoryDto>()
            .ForMember(dst => dst.LastPrice, opt => opt.MapFrom(src => Math.Round(src.LastPrice, 2)))
            .ForMember(dst => dst.MaxPrice, opt => opt.MapFrom(src => Math.Round(src.MaxPrice, 2)))
            .ForMember(dst => dst.MinPrice, opt => opt.MapFrom(src => Math.Round(src.MinPrice, 2)))
            .ForMember(dst => dst.AvgPrice, opt => opt.MapFrom(src => Math.Round(src.AvgPrice, 2)));
    }
}

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