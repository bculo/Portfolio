using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.ReadModels;
using FluentValidation;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries;

public record FetchPageQuery(string? Symbol) : PageBaseQuery, IRequest<PageBaseResult<FetchPageResponseDto>>;

public record FetchPageResponseDto
{
    public Guid Id { get; init; }
    public string Symbol { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Website { get; init; } = null!;
    public string SourceCode { get; init; } = null!;
    public decimal Price { get; init; }
}

public class FetchPageQueryValidator : AbstractValidator<FetchPageQuery>
{
    public FetchPageQueryValidator()
    {
        RuleFor(i => i.Take).GreaterThan(0);
        RuleFor(i => i.Page).GreaterThan(0);
    }
}

public class FetchPageQueryMapper : Profile
{
    public FetchPageQueryMapper()
    {
        CreateMap<CryptoLastPriceReadModel, FetchPageResponseDto>()
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.CryptoId))
            .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
            .ForMember(dst => dst.SourceCode, opt => opt.MapFrom(src => src.SourceCode))
            .ForMember(dst => dst.Website, opt => opt.MapFrom(src => src.Website))
            .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.LastPrice));

        CreateMap<FetchPageQuery, CryptoPricePageQuery>()
            .ForMember(dst => dst.Page, opt => opt.MapFrom(src => src.Page))
            .ForMember(dst => dst.Take, opt => opt.MapFrom(src => src.Take))
            .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol));
    }
}

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