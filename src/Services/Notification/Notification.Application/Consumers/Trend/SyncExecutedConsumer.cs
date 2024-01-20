using Events.Common.Trend;
using MassTransit;
using MediatR;
using Notification.Application.Features.Trend;

namespace Notification.Application.Consumers.Trend;

public class SyncExecutedConsumer : IConsumer<SyncExecuted>
{
    private readonly IMediator _mediator;

    public SyncExecutedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<SyncExecuted> context)
    {
        await _mediator.Send(new SendSyncExecutedNotificationCommand());
    }
}