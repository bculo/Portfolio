using AutoMapper;
using Crypto.Application.Common.Constants;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using Events.Common.Crypto;
using MassTransit;
using Microsoft.Extensions.Logging;
using ZiggyCreatures.Caching.Fusion;

namespace Crypto.Infrastructure.Consumers
{
    public class PriceUpdatedConsumer : IConsumer<PriceUpdated>
    {
        private readonly IMapper _mapper;
        private readonly IFusionCache _cache;
        private readonly ILogger<PriceUpdatedConsumer> _logger;

        public PriceUpdatedConsumer(ILogger<PriceUpdatedConsumer> logger, 
            IFusionCache cache, 
            IMapper mapper)
        {
            _logger = logger;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<PriceUpdated> context)
        {
            _logger.LogInformation("Updating item {Symbol}", context.Message.Symbol);
            
            var msg = context.Message;

            var dto = _mapper.Map<FetchSingleResponseDto>(msg);
            
            await _cache.SetAsync(CacheKeys.CryptoItemKey(msg.Symbol),
                dto,
                CacheKeys.CryptoItemKeyOptions());
        }
    }
}
