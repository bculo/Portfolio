using Events.Common.Trend;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Trend;

public class SyncExecutedConsumer(IMediator mediator) : IConsumer<SyncExecuted>
{
    public async Task Consume(ConsumeContext<SyncExecuted> context)
    {
        var message = context.Message;
        await mediator.Send(new SendSyncExecutedNotification(message.Time));
    }
}