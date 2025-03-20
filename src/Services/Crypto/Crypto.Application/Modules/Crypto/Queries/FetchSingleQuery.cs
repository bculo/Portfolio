using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Extensions;
using Crypto.Application.Common.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Exceptions;
using Crypto.Core.ReadModels;
using Events.Common.Crypto;
using FluentValidation;
using MassTransit;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Application.Modules.Crypto.Queries;

public record FetchSingleQuery : SymbolQuery, IRequest<FetchSingleResponseDto>;

public record FetchSingleResponseDto(
    Guid Id,
    string Symbol,
    string Name,
    decimal Price
)
{
    public static FetchSingleResponseDto Mapper(CryptoLastPriceReadModel m)
        => new(m.CryptoId, m.Symbol, m.Name, m.LastPrice);
}

public class FetchSingleQueryValidator : SymbolQueryValidator<FetchSingleQuery>;

public class FetchSingleQueryHandler(
    IUnitOfWork work,
    IPublishEndpoint publish,
    IFusionCache cache)
    : IRequestHandler<FetchSingleQuery, FetchSingleResponseDto>
{
    public async Task<FetchSingleResponseDto> Handle(FetchSingleQuery request, CancellationToken ct)
    {
        var item = await cache.GetOrSetAsync(CacheKeys.SingleItemKey(request.Symbol),
            async (token) => await GetRecord(request, token),
            CacheKeys.SingleItemKeyOptions(),
            ct);

        if (item == null)
            throw new CryptoCoreNotFoundException($"Item with symbol {request.Symbol} not found");
            
        await publish.Publish(new Visited
        {
            CryptoId = item.Id,
            Symbol = item.Symbol
        }, ct);
            
        return item;
    }

    private async Task<FetchSingleResponseDto?> GetRecord(FetchSingleQuery query, CancellationToken ct)
    {
        var instance = await work.CryptoPriceRepo.GetLastPrice(query.Symbol, ct);
        return instance is null 
            ? null 
            : FetchSingleResponseDto.Mapper(instance);
    }
}
