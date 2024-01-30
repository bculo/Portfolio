using FluentValidation;
using MediatR;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Expressions;
using Stock.Application.Interfaces.Expressions.Models;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record FilterStocks(string? Symbol) : PageRequestDto, IRequest<PageResultDto<FilterStockResponseItem>>;

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
    private readonly IExpressionBuilderFactory _factory;

    public FilterStocksHandler(IUnitOfWork work, 
        IExpressionBuilderFactory factory)
    {
        _work = work;
        _factory = factory;
    }

    public async Task<PageResultDto<FilterStockResponseItem>> Handle(
        FilterStocks request, 
        CancellationToken ct)
    {
        var expressionTree = BuildExpressionTree(request); 
        
        var page = await _work.StockWithPriceTag.PageMatchAll(
            expressionTree.Expressions,
            i => i.OrderBy(x => x.StockId),
            request.ToPageQuery(),
            ct: ct);

        return page.MapToDto(Projection);
    }

    private ExpressionBuildResult<StockWithPriceTagReadModel> BuildExpressionTree(FilterStocks request)
    {
        var builder = _factory.Create<StockWithPriceTagReadModel>();
        
        if (!string.IsNullOrEmpty(request.Symbol))
        {
            builder.Add(i => i.Symbol.Contains(request.Symbol));
        }

        return builder.Build();
    }

    public FilterStockResponseItem Projection(StockWithPriceTagReadModel entity)
    {
        return new FilterStockResponseItem(entity.StockId, entity.Symbol, entity.Price);
    }
}

public record FilterStockResponseItem(int Id, string Symbol, decimal Price);