using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Modules.Crypto.Queries;
using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Logging;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Infrastructure.Consumers
{
    public class PriceUpdatedConsumer(
        ILogger<PriceUpdatedConsumer> logger,
        IFusionCache cache,
        IMapper mapper)
        : IConsumer<CryptoPriceUpdated>
    {
        public async Task Consume(ConsumeContext<CryptoPriceUpdated> context)
        {
            logger.LogInformation("Updating item {Symbol}", context.Message.Symbol);
            
            var msg = context.Message;

            var dto = mapper.Map<FetchSingleResponseDto>(msg);
            
            await cache.SetAsync(CacheKeys.SingleItemKey(msg.Symbol),
                dto,
                CacheKeys.SingleItemKeyOptions());
        }
    }
}
