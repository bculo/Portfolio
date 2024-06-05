using Events.Common.Trend;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Trend;

public class ArticleDeactivatedConsumer(IMediator mediator) : IConsumer<ArticleDeactivated>
{
    public async Task Consume(ConsumeContext<ArticleDeactivated> context)
    {
        var message = context.Message;
        await mediator.Send(new ArticleStatusChangedNotification(message.ArticleId, false, message.Time));
    }
}