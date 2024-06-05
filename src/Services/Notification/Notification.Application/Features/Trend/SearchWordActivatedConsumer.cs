using Events.Common.Trend;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Trend;

public class SearchWordActivatedConsumer(IMediator mediator) : IConsumer<SearchWordActivated>
{
    public async Task Consume(ConsumeContext<SearchWordActivated> context)
    {
        var message = context.Message;
        await mediator.Publish(new SearchWordStatusChangedNotification(message.SearchWordId, message.Time));
    }
}