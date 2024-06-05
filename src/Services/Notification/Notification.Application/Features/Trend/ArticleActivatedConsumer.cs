using Events.Common.Trend;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Trend;

public class ArticleActivatedConsumer(IMediator mediator) : IConsumer<ArticleActivated>
{
    public async Task Consume(ConsumeContext<ArticleActivated> context)
    {
        var message = context.Message;
        await mediator.Send(new ArticleStatusChangedNotification(message.ArticleId, true, message.Time));
    }
}