using Events.Common.Crypto;
using MassTransit;

namespace Tracker.Application.Infrastructure.Consumers;

public class NewCryptoAddedConsumer : IConsumer<NewCryptoAdded>
{
    public Task Consume(ConsumeContext<NewCryptoAdded> context)
    {
        return Task.CompletedTask;
    }
}