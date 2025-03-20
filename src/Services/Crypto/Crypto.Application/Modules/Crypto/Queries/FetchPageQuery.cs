using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.ReadModels;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries;

public record FetchPageQuery(string? Symbol) : PageQuery, IRequest<Common.Models.PageResult<FetchPageResponseDto>>;
public class FetchPageQueryValidator : PageValidator<FetchPageQuery>;

public record FetchPageResponseDto(
    Guid Id,
    string Symbol,
    string Name,
    string Website,
    string SourceCode,
    decimal Price)
{
    public static FetchPageResponseDto From(CryptoLastPriceReadModel item)
        => new(item.CryptoId, item.Symbol, item.Name, item.Website, item.SourceCode, item.LastPrice);
}

public class FetchPageQueryMapper : Profile
{
    public FetchPageQueryMapper()
    {
        CreateMap<FetchPageQuery, CryptoPricePageRepoQuery>()
            .ForMember(dst => dst.Page, opt => opt.MapFrom(src => src.Page))
            .ForMember(dst => dst.Take, opt => opt.MapFrom(src => src.Take))
            .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol));
    }
}

public class FetchPageQueryHandler(IMapper mapper, IUnitOfWork work, IFusionCache cache)
    : IRequestHandler<FetchPageQuery, Common.Models.PageResult<FetchPageResponseDto>>
{
    public async Task<Common.Models.PageResult<FetchPageResponseDto>> Handle(FetchPageQuery request, CancellationToken ct)
    {
        var items = await cache.GetOrSetAsync(CacheKeys.FetchCryptoPageKey(request),
            async (token) => await GetAssets(request, token),
            CacheKeys.FetchCryptoPageKeyOptions(),
            ct);

        return items;
    }

    private async Task<Common.Models.PageResult<FetchPageResponseDto>> GetAssets(FetchPageQuery request, CancellationToken ct)
    {
        var query = mapper.Map<CryptoPricePageRepoQuery>(request);
        var repoResult = await work.CryptoPriceRepo.GetPage(query, ct);
        var dtoItems = repoResult.Items.Select(FetchPageResponseDto.From).ToList();
        return new Common.Models.PageResult<FetchPageResponseDto>(repoResult.TotalCount, request.Page, dtoItems);
    }
}