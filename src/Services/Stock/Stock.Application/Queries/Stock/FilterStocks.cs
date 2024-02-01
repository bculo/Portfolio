using FluentValidation;
using MediatR;
using Sqids;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Expressions;
using Stock.Application.Interfaces.Expressions.Models;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources;
using Stock.Core.Enums;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record FilterStocks(string? Symbol, Status Status) : PageRequestDto, IRequest<PageResultDto<FilterStockResponseItem>>;

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
    private readonly IExpressionBuilderFactory _factory;

    public FilterStocksHandler(IUnitOfWork work, 
        IExpressionBuilderFactory factory, 
        SqidsEncoder<int> sqids)
    {
        _work = work;
        _factory = factory;
        _sqids = sqids;
    }

    public async Task<PageResultDto<FilterStockResponseItem>> Handle(
        FilterStocks request, 
        CancellationToken ct)
    {
        var expressionTree = BuildExpressionTree(request); 
        
        var page = await _work.StockWithPriceTag.PageMatchAll(
            expressionTree.Expressions,
            i => i.OrderBy(x => x.Symbol),
            request.ToPageQuery(),
            ct: ct);

        return page.MapToDto(Projection);
    }

    private ExpressionBuildResult<StockWithPriceTag> BuildExpressionTree(FilterStocks request)
    {
        var builder = _factory.Create<StockWithPriceTag>();
        
        if (!string.IsNullOrEmpty(request.Symbol))
        {
            builder.Add(i => i.Symbol.Contains(request.Symbol));
        }

        if (request.Status != Status.All)
        {
            bool isActive = request.Status == Status.Active;
            builder.Add(i => i.IsActive == isActive);
        }

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