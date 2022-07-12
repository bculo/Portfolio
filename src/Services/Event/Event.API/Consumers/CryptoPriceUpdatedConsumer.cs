using Events.Common.Common;
using Events.Common.Crypto;
using MassTransit;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Event.API.Consumers
{
    public class CryptoPriceUpdatedConsumer : IConsumer<CryptoPriceUpdated>
    {
        public async Task Consume(ConsumeContext<CryptoPriceUpdated> context)
        {
            Debug.WriteLine(JsonConvert.SerializeObject(context.Message));
        }
    }
}
