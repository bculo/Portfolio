using Events.Common.Trend;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Trend;

public class ArticleDeactivatedConsumer : IConsumer<ArticleDeactivated>
{
    private readonly IMediator _mediator;
    
    public ArticleDeactivatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<ArticleDeactivated> context)
    {
        var message = context.Message;
        await _mediator.Send(new ArticleStatusChangedNotification(message.ArticleId, false, message.Time));
    }
}