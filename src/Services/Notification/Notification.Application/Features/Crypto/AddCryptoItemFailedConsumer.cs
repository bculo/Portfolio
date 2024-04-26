using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Notification.Application.Features.Crypto;

public class AddCryptoItemFailedConsumer : IConsumer<AddItemFailed>
{
    private readonly ILogger<AddCryptoItemFailedConsumer> _logger;

    public AddCryptoItemFailedConsumer(ILogger<AddCryptoItemFailedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<AddItemFailed> context)
    {
        return Task.CompletedTask;
    }
}