using Events.Common;
using Events.Common.Stock;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Exceptions;
using Stock.Core.Exceptions.Codes;
using Stock.Core.Models.Stock;
using Time.Common;

namespace Stock.Application.Commands.Stock;

public record ChangeStockStatus(Guid Id) : IRequest;

public class ChangeStockStatusHandler(
    IDataSourceProvider sourceProvider,
    IDateTimeProvider timeProvider,
    IPublishEndpoint publishEndpoint,
    IEntityManagerRepository managerRepository)
    : IRequestHandler<ChangeStockStatus>
{
    public async Task Handle(ChangeStockStatus request, CancellationToken ct)
    {
        var entity = await sourceProvider.GetQuery<StockEntity>()
            .SingleOrDefaultAsync(x => x.Id == request.Id, ct);
        
        if(entity == null)
            throw new StockCoreNotFoundException(StockErrorCodes.NotFoundById(request.Id));

        entity.ChangeStatus(timeProvider.TimeOffset);
        
        await PublishStatusChangedEvent(entity, ct);
        await managerRepository.SaveChanges(ct);
    }
    
    private Task PublishStatusChangedEvent(StockEntity entity, CancellationToken ct)
    {
        var @event = GetEvent(entity);
        return publishEndpoint.Publish(@event.Message, @event.MessageType, ct);
    }

    private IntegrationEvent GetEvent(StockEntity entity)
    {
        return entity.IsActive
            ? IntegrationEvent.From(new StockActivated(entity.Symbol, timeProvider.TimeOffset))
            : IntegrationEvent.From(new StockDeactivated(entity.Symbol, timeProvider.TimeOffset));
    } 
}