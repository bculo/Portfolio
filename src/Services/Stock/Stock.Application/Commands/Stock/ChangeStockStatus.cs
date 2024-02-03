using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Sqids;
using Stock.Application.Common.Configurations;
using Stock.Application.Common.Extensions;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Time.Abstract.Contracts;

namespace Stock.Application.Commands.Stock;

public record ChangeStockStatus(string Id) : IRequest;

public class ChangeStockStatusValidator : AbstractValidator<ChangeStockStatus>
{
    public ChangeStockStatusValidator()
    {
        RuleFor(i => i.Id)
            .NotEmpty();
    }
}

public class ChangeStockStatusHandler : IRequestHandler<ChangeStockStatus>
{
    private readonly SqidsEncoder<int> _sqids;
    private readonly IUnitOfWork _work;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IOutputCacheStore _outputCache;

    public ChangeStockStatusHandler(IUnitOfWork work, 
        SqidsEncoder<int> sqids, 
        IDateTimeProvider timeProvider, 
        IOutputCacheStore outputCache)
    {
        _work = work;
        _sqids = sqids;
        _timeProvider = timeProvider;
        _outputCache = outputCache;
    }
    
    public async Task Handle(ChangeStockStatus request, CancellationToken ct)
    {
        var entityId = _sqids.DecodeSingle(request.Id);
        if (entityId == default(long))
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundById(request.Id));
        }

        var entity = await _work.StockRepo.Find(entityId, ct);
        if (entity is null)
        {
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundById(request.Id));
        }

        entity.IsActive = !entity.IsActive;
        if (entity.IsActive) entity.Deactivated = null;
        else entity.Deactivated = _timeProvider.Now;
        await _work.Save(ct);

        await _outputCache.EvictByTagAsync(request.Id, ct);
        await _outputCache.EvictByTagAsync(CacheTags.STOCK_FILTER, ct);
    }
}