using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Queryable.Common.Extensions;
using Queryable.Common.Models;
using Queryable.Common.Services.Dynamic;
using Sqids;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources;
using Stock.Core.Enums;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record FilterStocks(
        ContainFilter? Symbol,
        GreaterThanFilter<decimal>? PriceGreaterThan,
        LessThenFilter<decimal>? PriceLessThan,
        EqualFilter<Status> ActivityStatus,
        GreaterThanFilter<DateTimeOffset>? NotOlderThan,
        SortBy? SortBy) 
    : PageRequestDto, IRequest<PageResultDto<FilterStockResponseItem>>;

public class FilterStocksValidator : AbstractValidator<FilterStocks>
{
    private readonly HashSet<string> _allowedSortProperties =
    [
        nameof(StockWithPriceTag.Symbol),
        nameof(StockWithPriceTag.Price)
    ];
    
    public FilterStocksValidator(ILocale locale)
    {
        Include(new PageRequestDtoValidator());

        When(i => i.Symbol is not null, () =>
        {
            RuleFor(i => i.Symbol!.Value)!
                .MatchesStockSymbolWhen(i => !string.IsNullOrEmpty(i.Symbol))
                .WithMessage(locale.Get(ValidationShared.STOCK_SYMBOL_PATTERN));
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

        When(i => i.SortBy is not null, () =>
        {
            RuleFor(i => i.SortBy!.PropertyName)
                .Must(x => _allowedSortProperties.Contains(x))
                .NotEmpty();
        });
    }
}

public class FilterStocksHandler(IDataSourceProvider provider) 
    : IRequestHandler<FilterStocks, PageResultDto<FilterStockResponseItem>>
{
    private readonly IQueryable<StockWithPriceTag> _query = provider.GetQuery<StockWithPriceTag>();
    
    public async Task<PageResultDto<FilterStockResponseItem>> Handle(
        FilterStocks request, 
        CancellationToken ct)
    {
        var expressions = BuildExpressionTree(request);

        var page = await _query.ApplyWhereAll(expressions).ToListAsync(ct);

        throw new NotImplementedException();
        //return page.MapToDto(Projection);
    }
    
    private Expression<Func<StockWithPriceTag, bool>>[] BuildExpressionTree(FilterStocks request)
    {
        var builder = DynamicExpressionBuilder<StockWithPriceTag>.Create();
        
        builder.Add(i => i.Symbol, request.Symbol)
            .Add(i => i.Price, request.PriceLessThan)
            .Add(i => i.Price, request.PriceGreaterThan)
            .Add(i => i.CreatedAt, request.NotOlderThan)
            .Add(i => i.IsActive, request.ActivityStatus.Value.ToEqualFilter());
        
        return builder.Build();
    }

    private FilterStockResponseItem Projection(StockWithPriceTag item)
    {
        return new FilterStockResponseItem
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

public record FilterStockResponseItem : StockPriceResultDto;