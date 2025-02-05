using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Commands.Cache;

public record RefreshStockItemValue(Guid Id, string Symbol, decimal Price) : IRequest;

public class RefreshStockItemValueHandler(
    IFusionCache cache,
    IOutputCacheStore outputCache,
    IDataSourceProvider queryProvider)
    : IRequestHandler<RefreshStockItemValue>
{
    public async Task Handle(RefreshStockItemValue request, CancellationToken ct)
    {
        var item = await queryProvider.GetReadOnlySourceQuery<StockWithPriceTag>()
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(i => i.StockId == request.Id, ct);

        if (item == null)
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundBySymbol(request.Symbol));
        
        await outputCache.EvictByTagAsync(request.Symbol, ct);
    }
}