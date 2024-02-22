using Crypto.Application.Common.Constants;
using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Logging;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Infrastructure.Consumers
{
    public class PriceUpdatedConsumer : IConsumer<PriceUpdated>
    {
        private readonly ILogger<PriceUpdatedConsumer> _logger;
        private readonly IFusionCache _cache;

        public PriceUpdatedConsumer(ILogger<PriceUpdatedConsumer> logger, 
            IFusionCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<PriceUpdated> context)
        {
            var msg = context.Message;
            
            await _cache.SetAsync(CacheKeys.CryptoItemKey(msg.Symbol),
                new { msg.Symbol, msg.Price, msg.Name },
                CacheKeys.CryptoItemKeyOptions(),
                default);
        }
    }
}
