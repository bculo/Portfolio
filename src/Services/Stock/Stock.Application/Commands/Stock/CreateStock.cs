using Events.Common.Stock;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    IDataSourceProvider provider,
    IPublishEndpoint publish,
    IEntityManagerRepository managerRepository)
    : IRequestHandler<CreateStock, Guid>
{
    public async Task<Guid> Handle(CreateStock request, CancellationToken ct)
    {
        var instance = await provider.GetQuery<StockEntity>()
            .SingleOrDefaultAsync(x => x.Symbol.ToLower() == request.Symbol.ToLower(), ct);
        
        if (instance is not null)
            throw new StockCoreException(StockErrorCodes.Duplicate(request.Symbol));

        var priceResponse = await client.GetPrice(request.Symbol, ct);
        if (priceResponse is null)
            throw new StockCoreException(StockErrorCodes.NotSupported(request.Symbol));

        var newStockItem = StockEntity.NewWithPrice(request.Symbol, priceResponse.Price);
        await managerRepository.Add(newStockItem, ct);
        
        await publish.Publish(new NewStockItemAdded
        {
            Created = newStockItem.CreatedAt,
            Price = priceResponse.Price,
            Symbol = newStockItem.Symbol
        }, ct);

        await managerRepository.SaveChanges(ct);
        return newStockItem.Id;
    }
}
