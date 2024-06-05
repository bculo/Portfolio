using Events.Common.Mail;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Mail;

public class CustomMailSentConsumer(IMediator mediator) : IConsumer<CustomMailSent>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<CustomMailSent> context)
    {
        var instance = context.Message;
        
    }
}