using Events.Common.Trend;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Trend;

public class SearchWordDeactivatedConsumer(IMediator mediator) : IConsumer<SearchWordDeactivated>
{
    public async Task Consume(ConsumeContext<SearchWordDeactivated> context)
    {
        var message = context.Message;
        await mediator.Publish(new SearchWordStatusChangedNotification(message.SearchWordId, message.Time));
    }
}