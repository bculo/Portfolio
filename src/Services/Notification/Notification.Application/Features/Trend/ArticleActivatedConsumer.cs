using Events.Common.Trend;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Trend;

public class ArticleActivatedConsumer : IConsumer<ArticleActivated>
{
    private readonly IMediator _mediator;
    
    public ArticleActivatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<ArticleActivated> context)
    {
        var message = context.Message;
        await _mediator.Send(new ArticleStatusChangedNotification(message.ArticleId, true, message.Time));
    }
}