using Events.Common.Crypto;
using MassTransit;

namespace Tracker.Application.Infrastructure.Consumers;

public class CryptoPriceUpdatedConsumer : IConsumer<CryptoPriceUpdated>
{
    public Task Consume(ConsumeContext<CryptoPriceUpdated> context)
    {
        return Task.CompletedTask;
    }
}