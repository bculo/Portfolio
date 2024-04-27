using Events.Common.Trend;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Trend;

public class SearchWordActivatedConsumer : IConsumer<SearchWordActivated>
{
    private readonly IMediator _mediator;
    
    public SearchWordActivatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<SearchWordActivated> context)
    {
        var message = context.Message;
        await _mediator.Publish(new SearchWordStatusChangedNotification(message.SearchWordId, message.Time));
    }
}