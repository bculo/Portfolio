using Events.Common.Mail;
using MassTransit;
using MediatR;
using Notification.Application.EventHandlers.Mail;

namespace Notification.Application.Consumers.Mail;

public class CustomMailSentConsumer : IConsumer<CustomMailSent>
{
    private readonly IMediator _mediator;

    public CustomMailSentConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<CustomMailSent> context)
    {
        var instance = context.Message;

        await _mediator.Publish(new CustomMailSentUserNotification.Notification
        {
            Date = instance.SentDate,
            MailId = instance.MailId,
            UserId = instance.UserId
        });
    }
}