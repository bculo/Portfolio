using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Crypto;

public class CryptoPriceUpdatedConsumer : IConsumer<CryptoPriceUpdated>
{
    private readonly IMediator _mediator;

    public CryptoPriceUpdatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<CryptoPriceUpdated> context)
    {
        var instance = context.Message;
        var command = new SendPriceUpdatedNotification(instance.Symbol, instance.Price);
        await _mediator.Send(command);
    }
}