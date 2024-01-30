using FluentValidation;
using MediatR;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources;
using Stock.Core.Models.Stock;

namespace Stock.Application.Queries.Stock;

public record FilterStocks(string Symbol) : PageRequestDto, IRequest<PageResultDto<FilterStockResponseItem>>;

public class FilterStocksValidator : AbstractValidator<FilterStocks>
{
    public FilterStocksValidator(ILocale locale)
    {
        Include(new PageRequestDtoValidator());

        RuleFor(i => i.Symbol)
            .NotEmpty()
            .MatchesStockSymbolWhen(i => !string.IsNullOrEmpty(i.Symbol))
            .WithMessage(locale.Get(ValidationShared.STOCK_SYMBOL_PATTERN));
    }
}

public class FilterStocksHandler : IRequestHandler<FilterStocks, PageResultDto<FilterStockResponseItem>>
{
    private readonly IUnitOfWork _work;

    public FilterStocksHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task<PageResultDto<FilterStockResponseItem>> Handle(
        FilterStocks request, 
        CancellationToken ct)
    {
        var page = await _work.StockRepo.Page(
            i => i.Symbol.Contains(request.Symbol),
            i => i.OrderBy(x => x.CreatedAt),
            request.ToPageQuery(),
            ct: ct);

        return page.MapToDto(Projection);
    }

    public FilterStockResponseItem Projection(StockEntity entity)
    {
        return new FilterStockResponseItem(entity.Id, entity.Symbol);
    }
}

public record FilterStockResponseItem(int Id, string Symbol);