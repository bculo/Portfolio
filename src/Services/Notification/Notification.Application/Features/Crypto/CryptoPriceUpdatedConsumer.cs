using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Notification.Application.Features.Crypto;

public class CryptoPriceUpdatedConsumer(ISender mediator) : IConsumer<CryptoPriceUpdated>
{
    public async Task Consume(ConsumeContext<CryptoPriceUpdated> context)
    {
        var instance = context.Message;
        var command = new SendPriceUpdatedNotification(instance.Symbol, instance.Price);
        await mediator.Send(command);
    }
}