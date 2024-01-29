using System.Text.RegularExpressions;
using Events.Common.Stock;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Localization;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources.Shared;
using Stock.Core.Exceptions;
using Stock.Core.Models;
using Stock.Core.Models.Stock;

namespace Stock.Application.Commands.Stock;

public record CreateStock(string Symbol) : IRequest<long>;

public class CreateStockValidator : AbstractValidator<CreateStock>
{
    public CreateStockValidator(IStringLocalizer<ValidationShared> localizer)
    {
        RuleFor(i => i.Symbol)
            .Matches(new Regex("^[a-zA-Z]{1,10}$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                TimeSpan.FromSeconds(1)))
            .WithMessage(localizer.GetString("Symbol pattern not valid"))
            .NotEmpty();
    }
}

public class CreateStockHandler : IRequestHandler<CreateStock, long>
{
    private readonly IStockPriceClient _client;
    private readonly IPublishEndpoint _publish;
    private readonly IStringLocalizer<CreateStockLocale> _locale;
    private readonly IBaseRepository<StockEntity> _repo;

    public CreateStockHandler(IStockPriceClient client,
        IPublishEndpoint publish,
        IBaseRepository<StockEntity> repo,
        IStringLocalizer<CreateStockLocale> locale)
    {
        _repo = repo;
        _client = client;
        _publish = publish;
        _locale = locale;
    }

    public async Task<long> Handle(CreateStock request, CancellationToken cancellationToken)
    {
        var existingInstance = await _repo.First(i => i.Symbol.ToLower() == request.Symbol.ToLower());
        if (existingInstance is not null)
        {
            string exceptionMessage = string.Format(_locale.GetString("Symbol already exists"), request.Symbol);
            throw new StockCoreException(exceptionMessage);
        }

        var clientResult = await _client.GetPrice(request.Symbol);
        if (clientResult is null)
        {
            string exceptionMessage = string.Format(_locale.GetString("Symbol not supported"), request.Symbol);
            throw new StockCoreException(exceptionMessage);
        }

        var newItem = new StockEntity { Symbol = request.Symbol };
        await _repo.Add(newItem);
        await _repo.SaveChanges();

        await _publish.Publish(new NewStockItemAdded
        {
            Created = newItem.CreatedAt,
            Symbol = newItem.Symbol
        });

        return newItem.Id;
    }
}

public class CreateStockLocale { }

