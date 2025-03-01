using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Queryable.Common.Extensions;
using Queryable.Common.Models;
using Queryable.Common.Services.Dynamic;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources;
using Stock.Core.Enums;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record FilterStocksQuery(
    ContainFilter? Symbol,
    GreaterThanFilter<decimal>? PriceGreaterThan,
    LessThenFilter<decimal>? PriceLessThan,
    EqualFilter<Status> ActivityStatus,
    GreaterThanFilter<DateTimeOffset>? NotOlderThan,
    Sort<FilterStockSortingOptions> SortBy)
    : PageRequestDto, IRequest<PaginatedResult<FilterStockResponse>>;

public enum FilterStockSortingOptions
{
    Symbol,
    Price
}

public class FilterStocksQueryValidator : AbstractValidator<FilterStocksQuery>
{
    public FilterStocksQueryValidator(ILocale locale)
    {
        Include(new PageRequestDtoValidator());

        When(i => i.Symbol is not null, () =>
        {
            RuleFor(i => i.Symbol!.Value)!
                .MatchesStockSymbolWhen(i => !string.IsNullOrEmpty(i.Symbol))
                .WithMessage(locale.Get(ValidationShared.StockSymbolPattern));
        });

        When(i => i.PriceLessThan is not null, () =>
        {
            RuleFor(i => i.PriceLessThan!.Value)!
                .GreaterThanOrEqualTo(0m);
        });
        
        When(i => i.PriceGreaterThan is not null, () =>
        {
            RuleFor(i => i.PriceGreaterThan!.Value)!
                .GreaterThanOrEqualTo(0m);
        });
    }
}

public class FilterStocksQueryHandler(IDataSourceProvider provider) 
    : IRequestHandler<FilterStocksQuery, PaginatedResult<FilterStockResponse>>
{
    private static readonly SortDefinition<FilterStockSortingOptions, StockWithPriceTag> SortDefinitions = new()
    {
        { FilterStockSortingOptions.Price, x => x.Price },
        { FilterStockSortingOptions.Symbol, x => x.Symbol },
    };
    
    private readonly IQueryable<StockWithPriceTag> _querySource = provider.GetReadOnlySourceQuery<StockWithPriceTag>();
    
    public async Task<PaginatedResult<FilterStockResponse>> Handle(FilterStocksQuery request, CancellationToken ct)
    {
        var expressions = BuildExpressionTree(request);

        var page = await _querySource.ApplyWhereAll(expressions)
            .ApplySortBy(request.SortBy, SortDefinitions)
            .AsPaginatedResult(request);

        return page.MapTo(ResponseProjection);
    }
    
    private Expression<Func<StockWithPriceTag, bool>>[] BuildExpressionTree(FilterStocksQuery request)
    {
        var builder = DynamicExpressionBuilder<StockWithPriceTag>.Create();
        
        builder.Add(i => i.Symbol, request.Symbol)
            .Add(i => i.Price, request.PriceLessThan)
            .Add(i => i.Price, request.PriceGreaterThan)
            .Add(i => i.CreatedAt, request.NotOlderThan)
            .Add(i => i.IsActive, request.ActivityStatus.Value.ToEqualFilter());
        
        return builder.Build();
    }

    private FilterStockResponse ResponseProjection(StockWithPriceTag item)
    {
        return new FilterStockResponse
        {
            LastPriceUpdate = item.LastPriceUpdate,
            Price = item.Price,
            Symbol = item.Symbol,
            IsActive = item.IsActive,
            Id = item.StockId,
            Created = item.CreatedAt
        };
    }
}

public record FilterStockResponse : StockPriceResultDto;