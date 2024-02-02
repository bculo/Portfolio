using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Queryable.Common.Models;
using Queryable.Common.Services.Classic;
using Queryable.Common.Services.Dynamic;
using Sqids;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Enums;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record FilterStocks(
        ContainFilter? Symbol,
        GreaterThanFilter<decimal>? PriceGreaterThan,
        LessThenFilter<decimal>? PriceLessThan,
        EqualFilter<Status> ActivityStatus,
        GreaterThanFilter<DateTime>? NotOlderThan) 
    : PageRequestDto, IRequest<PageResultDto<FilterStockResponseItem>>;

public class FilterStocksValidator : AbstractValidator<FilterStocks>
{
    public FilterStocksValidator(ILocale locale)
    {
        Include(new PageRequestDtoValidator());
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
        var builder = DynamicExpressionBuilder<StockWithPriceTag>.Create();

        builder.Add(i => i.Symbol, request.Symbol)
            .Add(i => i.Price, request.PriceLessThan)
            .Add(i => i.Price, request.PriceGreaterThan)
            .Add(i => i.CreatedAt, request.NotOlderThan);

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