using Events.Common.Crypto;
using MassTransit;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Event.API.Consumers
{
    public class CryptoInfoUpdatedConsumer : IConsumer<CryptoInfoUpdated>
    {
        public async Task Consume(ConsumeContext<CryptoInfoUpdated> context)
        {
            Debug.WriteLine(JsonConvert.SerializeObject(context.Message));
        }
    }
}
