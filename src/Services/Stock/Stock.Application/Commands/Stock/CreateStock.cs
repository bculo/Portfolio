using Events.Common.Stock;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Sqids;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Extensions;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;

namespace Stock.Application.Commands.Stock;

public record CreateStock(string Symbol) : IRequest<Guid>;

public class CreateStockValidator : AbstractValidator<CreateStock>
{
    public CreateStockValidator(ILocale locale)
    {
        RuleFor(i => i.Symbol)
            .NotEmpty()
            .MatchesStockSymbolWhen(i => !string.IsNullOrEmpty(i.Symbol))
            .WithMessage(locale.Get(ValidationShared.StockSymbolPattern));
    }
}

public class CreateStockHandler(
    IStockPriceClient client,
    IPublishEndpoint publish,
    IOutputCacheStore outputCache)
    : IRequestHandler<CreateStock, Guid>
{
    public async Task<Guid> Handle(CreateStock request, CancellationToken ct)
    {
        var instance = await work.StockRepo.First(
            i => i.Symbol.ToLower() == request.Symbol.ToLower(),
            ct: ct);
        if (instance is not null)
            throw new StockCoreException(StockErrorCodes.Duplicate(request.Symbol));

        var clientResult = await client.GetPrice(request.Symbol, ct);
        if (clientResult is null)
            throw new StockCoreException(StockErrorCodes.NotSupported(request.Symbol));

        var newItem = new StockEntity
        {
            Symbol = request.Symbol,
            IsActive = true,
            Prices = new List<StockPriceEntity>()
            {
                new()
                {
                    Price = clientResult.Price,
                }
            }
        };
        await work.StockRepo.Add(newItem, ct);
        
        await publish.Publish(new NewStockItemAdded
        {
            Created = newItem.CreatedAt,
            Price = clientResult.Price,
            Symbol = newItem.Symbol
        }, ct);

        await work.Save(ct);
        
        await outputCache.EvictByTagAsync(CacheTags.StockFilter, ct);
        
        return sqids.Encode(newItem.Id);
    }
}
