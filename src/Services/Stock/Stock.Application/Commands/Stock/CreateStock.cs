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

public record CreateStock(string Symbol) : IRequest<string>;

public class CreateStockValidator : AbstractValidator<CreateStock>
{
    public CreateStockValidator(ILocale locale)
    {
        RuleFor(i => i.Symbol)
            .NotEmpty()
            .MatchesStockSymbolWhen(i => !string.IsNullOrEmpty(i.Symbol))
            .WithMessage(locale.Get(ValidationShared.STOCK_SYMBOL_PATTERN));
    }
}

public class CreateStockHandler : IRequestHandler<CreateStock, string>
{
    private readonly IStockPriceClient _client;
    private readonly IPublishEndpoint _publish;
    private readonly IUnitOfWork _work;
    private readonly SqidsEncoder<int> _sqids;
    private readonly IOutputCacheStore _outputCache;

    public CreateStockHandler(IStockPriceClient client,
        IPublishEndpoint publish,
        IUnitOfWork work, 
        SqidsEncoder<int> sqids, 
        IOutputCacheStore outputCache)
    {
        _work = work;
        _sqids = sqids;
        _outputCache = outputCache;
        _client = client;
        _publish = publish;
    }

    public async Task<string> Handle(CreateStock request, CancellationToken ct)
    {
        var instance = await _work.StockRepo.First(
            i => i.Symbol.ToLower() == request.Symbol.ToLower(),
            ct: ct);
        if (instance is not null)
        {
            throw new StockCoreException(StockErrorCodes.Duplicate(request.Symbol));
        }

        var clientResult = await _client.GetPrice(request.Symbol, ct);
        if (clientResult is null)
        {
            throw new StockCoreException(StockErrorCodes.NotSupported(request.Symbol));
        }

        var newItem = new StockEntity
        {
            Symbol = request.Symbol,
            IsActive = true
        };
        await _work.StockRepo.Add(newItem, ct);
        await _work.Save(ct);

        await _publish.Publish(new NewStockItemAdded
        {
            Created = newItem.CreatedAt,
            Symbol = newItem.Symbol
        }, ct);

        await _outputCache.EvictByTagAsync(CacheTags.STOCK_FILTER, ct);
        
        return _sqids.Encode(newItem.Id);
    }
}
