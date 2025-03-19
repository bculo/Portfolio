using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Extensions;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Exceptions;
using Crypto.Core.ReadModels;
using Events.Common.Crypto;
using FluentValidation;
using MassTransit;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries;

public record FetchSingleQuery(string Symbol) : IRequest<FetchSingleResponseDto>;

public record FetchSingleResponseDto
{
    public Guid Id { get; init; }
    public string Symbol { get; init; } = null!;
    public string Name { get; init; } = null!;
    public decimal Price { get; init; }
}

public class FetchSingleQueryValidator : AbstractValidator<FetchSingleQuery>
{
    public FetchSingleQueryValidator()
    {
        RuleFor(i => i.Symbol).WithSymbolRule();
    }
}

public class FetchSingleQueryMapper : Profile
{
    public FetchSingleQueryMapper()
    {
        CreateMap<CryptoPriceUpdated, FetchSingleResponseDto>()
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
            .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price));
            
        CreateMap<CryptoLastPriceReadModel, FetchSingleResponseDto>()
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.CryptoId))
            .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
            .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.LastPrice));
    }
}

public class FetchSingleQueryHandler(
    IMapper mapper,
    IUnitOfWork work,
    IPublishEndpoint publish,
    IFusionCache cache)
    : IRequestHandler<FetchSingleQuery, FetchSingleResponseDto>
{
    public async Task<FetchSingleResponseDto> Handle(FetchSingleQuery request, CancellationToken ct)
    {
        var item = await cache.GetOrSetAsync(CacheKeys.SingleItemKey(request.Symbol),
            async (token) =>
            {
                var instance = await work.CryptoPriceRepo.GetLastPrice(request.Symbol, token);
                return instance is null ? null : mapper.Map<FetchSingleResponseDto>(instance);
            },
            CacheKeys.SingleItemKeyOptions(),
            ct);

        if (item == null)
        {
            throw new CryptoCoreNotFoundException($"Item with symbol {request.Symbol} not found");
        }
            
        await publish.Publish(new Visited
        {
            CryptoId = item.Id,
            Symbol = item.Symbol
        }, ct);
            
        return item;
    }
}
