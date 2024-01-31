using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Sqids;
using Stock.Application.Common.Configurations;
using Stock.Application.Common.Extensions;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Queries.Stock;


public record GetStock(string Id) : IRequest<GetStockResponse>;

public class GetStockValidator : AbstractValidator<GetStock>
{
    public GetStockValidator(ILocale locale)
    {
        RuleFor(i => i.Id)
            .NotEmpty();
    }
}

public class GetStockHandler : IRequestHandler<GetStock, GetStockResponse>
{
    private readonly IUnitOfWork _work;
    private readonly SqidsEncoder<int> _sqids;
    private readonly IFusionCache _cache;
    private readonly ILogger<GetStockHandler> _logger;

    public GetStockHandler(ILogger<GetStockHandler> logger, 
        IUnitOfWork work, 
        SqidsEncoder<int> sqids,
        IFusionCache cache)
    {
        _logger = logger;
        _work = work;
        _sqids = sqids;
        _cache = cache;
    }
    
    public async Task<GetStockResponse> Handle(GetStock request, CancellationToken ct)
    {
        var entityId = _sqids.DecodeSingle(request.Id);
        if (entityId == default(long))
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundById(entityId));
        }
        
        var entity = await _cache.GetOrSetAsync(
            CacheKeys.StockItemKey(entityId),
            token => _work.StockRepo.Find(entityId, token),
            CacheKeys.StockItemKeyOptions(),
            ct
        );
        if(entity is null)
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundById(entityId));
        }
        
        return Map(entity);
    }

    private GetStockResponse Map(StockEntity entity)
    {
        return new GetStockResponse(Id: entity.Id, Symbol: entity.Symbol);
    }
}

public record GetStockResponse(long Id, string Symbol);