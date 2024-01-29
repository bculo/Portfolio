using System.Text.RegularExpressions;
using Events.Common.Stock;
using FluentValidation;
using LanguageExt;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Localization;
using Stock.Application.Common.Extensions;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources.Shared;
using Stock.Core.Errors;
using Stock.Core.Exceptions;
using Stock.Core.Models;
using Stock.Core.Models.Stock;

namespace Stock.Application.Commands.Stock;

public record CreateStock(string Symbol) : IRequest<long>;

public class CreateStockValidator : AbstractValidator<CreateStock>
{
    public CreateStockValidator(IStringLocalizer<ValidationShared> locale)
    {
        RuleFor(i => i.Symbol)
            .MatchesStockSymbol()
            .WithMessage(locale.GetString("Symbol pattern not valid"))
            .NotEmpty();
    }
}

public class CreateStockHandler : IRequestHandler<CreateStock, long>
{
    private readonly IStockPriceClient _client;
    private readonly IPublishEndpoint _publish;
    private readonly IStringLocalizer<CreateStockLocale> _locale;
    private readonly IUnitOfWork _work;

    public CreateStockHandler(IStockPriceClient client,
        IPublishEndpoint publish,
        IUnitOfWork work,
        IStringLocalizer<CreateStockLocale> locale)
    {
        _work = work;
        _client = client;
        _publish = publish;
        _locale = locale;
    }

    public async Task<long> Handle(CreateStock request, CancellationToken ct)
    {
        var instance = await _work.StockRepo.First(
            i => i.Symbol.ToLower() == request.Symbol.ToLower(), 
            ct);
        if (instance is not null)
        {
            throw new StockCoreException(StockErrorCodes.Duplicate(request.Symbol));
        }

        var clientResult = await _client.GetPrice(request.Symbol);
        if (clientResult is null)
        {
            throw new StockCoreException(StockErrorCodes.NotSupported(request.Symbol));
        }

        var newItem = new StockEntity
        {
            Symbol = request.Symbol
        };
        await _work.StockRepo.Add(newItem, ct);
        await _work.Save(ct);

        await _publish.Publish(new NewStockItemAdded
        {
            Created = newItem.CreatedAt,
            Symbol = newItem.Symbol
        }, ct);

        return newItem.Id;
    }
}

public class CreateStockLocale { }

