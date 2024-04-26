using Events.Common.Trend;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Trend;

public class SyncExecutedConsumer : IConsumer<SyncExecuted>
{
    private readonly IMediator _mediator;

    public SyncExecutedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<SyncExecuted> context)
    {
        var message = context.Message;
        await _mediator.Send(new SendSyncExecutedNotification(message.Time));
    }
}