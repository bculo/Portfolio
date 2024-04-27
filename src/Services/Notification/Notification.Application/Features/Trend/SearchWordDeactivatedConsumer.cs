using Events.Common.Trend;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Trend;

public class SearchWordDeactivatedConsumer : IConsumer<SearchWordDeactivated>
{
    private readonly IMediator _mediator;
    
    public SearchWordDeactivatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<SearchWordDeactivated> context)
    {
        var message = context.Message;
        await _mediator.Publish(new SearchWordStatusChangedNotification(message.SearchWordId, message.Time));
    }
}