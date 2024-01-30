using Cache.Abstract.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sqids;
using Stock.Application.Common.Extensions;
using Stock.Application.Interfaces.Localization;
using Stock.Application.Interfaces.Repositories;
using Stock.Application.Resources;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;

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
    private readonly ICacheService _cache;
    private readonly IUnitOfWork _work;
    private readonly SqidsEncoder<int> _sqids;
    private readonly ILogger<GetStockHandler> _logger;

    public GetStockHandler(ICacheService cache,
        ILogger<GetStockHandler> logger, 
        IUnitOfWork work, 
        SqidsEncoder<int> sqids)
    {
        _cache = cache;
        _logger = logger;
        _work = work;
        _sqids = sqids;
    }
    
    public async Task<GetStockResponse> Handle(GetStock request, CancellationToken ct)
    {
        var entityId = _sqids.DecodeSingle(request.Id);
        var entity = await _work.StockRepo.Find(entityId, ct);
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