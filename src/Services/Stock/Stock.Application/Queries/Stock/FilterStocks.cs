using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Queryable.Common.Services.Classic;
using Sqids;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources;
using Stock.Core.Enums;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record FilterStocks(string? Symbol, Status Status) 
    : PageRequestDto, IRequest<PageResultDto<FilterStockResponseItem>>;

public class FilterStocksValidator : AbstractValidator<FilterStocks>
{
    public FilterStocksValidator(ILocale locale)
    {
        Include(new PageRequestDtoValidator());
        
        RuleFor(i => i.Symbol)!
            .MatchesStockSymbolWhen(i => !string.IsNullOrEmpty(i.Symbol))
            .WithMessage(locale.Get(ValidationShared.STOCK_SYMBOL_PATTERN));
    }
}

public class FilterStocksHandler : IRequestHandler<FilterStocks, PageResultDto<FilterStockResponseItem>>
{
    private readonly IUnitOfWork _work;
    private readonly SqidsEncoder<int> _sqids;

    public FilterStocksHandler(IUnitOfWork work,
        SqidsEncoder<int> sqids)
    {
        _work = work;
        _sqids = sqids;
    }

    public async Task<PageResultDto<FilterStockResponseItem>> Handle(
        FilterStocks request, 
        CancellationToken ct)
    {
        var expressions = BuildExpressionTree(request); 
        
        var page = await _work.StockWithPriceTag.PageMatchAll(
            expressions,
            i => i.OrderBy(x => x.Symbol),
            request.ToPageQuery(),
            ct: ct);

        return page.MapToDto(Projection);
    }

    private Expression<Func<StockWithPriceTag, bool>>[] BuildExpressionTree(FilterStocks request)
    {
        var builder = ExpressionBuilder<StockWithPriceTag>.Create();

        builder.Add(i => i.Symbol.Contains(request.Symbol), !string.IsNullOrEmpty(request.Symbol))
            .Add(i => i.IsActive == (request.Status == Status.Active), request.Status != Status.None);
        
        return builder.Build();
    }

    private FilterStockResponseItem Projection(StockWithPriceTag item)
    {
        return new FilterStockResponseItem
        {
            LastPriceUpdate = item.LastPriceUpdate,
            Price = item.Price == -1 ? default(decimal?) : item.Price,
            Symbol = item.Symbol,
            IsActive = item.IsActive,
            Id = _sqids.Encode(item.StockId),
            Created = item.CreatedAt
        };
    }
}

public record FilterStockResponseItem : StockPriceResultDto;