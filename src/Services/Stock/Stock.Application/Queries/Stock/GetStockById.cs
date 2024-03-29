using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Sqids;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Models;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;
using ZiggyCreatures.Caching.Fusion;

namespace Stock.Application.Queries.Stock;


public record GetStockById(string Id) : IRequest<GetStockByIdResponse>;

public class GetStockByIdValidator : AbstractValidator<GetStockById>
{
    public GetStockByIdValidator(ILocale locale)
    {
        RuleFor(i => i.Id)
            .NotEmpty();
    }
}

public class GetStockByIdHandler : IRequestHandler<GetStockById, GetStockByIdResponse>
{
    private readonly IUnitOfWork _work;
    private readonly SqidsEncoder<int> _sqids;
    private readonly IFusionCache _cache;
    private readonly ILogger<GetStockByIdHandler> _logger;

    public GetStockByIdHandler(ILogger<GetStockByIdHandler> logger, 
        IUnitOfWork work, 
        SqidsEncoder<int> sqids,
        IFusionCache cache)
    {
        _logger = logger;
        _work = work;
        _sqids = sqids;
        _cache = cache;
    }
    
    public async Task<GetStockByIdResponse> Handle(GetStockById request, CancellationToken ct)
    {
        var entityId = _sqids.DecodeSingle(request.Id);
        if (entityId == default(long))
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundById(request.Id));
        }

        var entity = await _cache.GetOrSetAsync(
            CacheKeys.StockItemKey(entityId),
            token => _work.StockWithPriceTag.First(i => i.StockId == entityId, ct: token),
            CacheKeys.StockItemKeyOptions(),
            ct
        );
        if(entity is null)
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundById(request.Id));
        }
        
        return Map(entity);
    }

    private GetStockByIdResponse Map(StockWithPriceTag item)
    {
        return new GetStockByIdResponse
        {
            LastPriceUpdate = item.LastPriceUpdate,
            Price = item.Price == -1 ? default(double?) : (double)item.Price,
            Symbol = item.Symbol,
            IsActive = item.IsActive,
            Id = _sqids.Encode(item.StockId),
            Created = item.CreatedAt
        };
    }
}

public record GetStockByIdResponse : StockPriceResultDto;