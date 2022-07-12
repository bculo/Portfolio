using Events.Common.Crypto;
using MassTransit;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Event.API.Consumers
{
    public class NewCryptoAddedConsumer : IConsumer<NewCryptoAdded>
    {
        public async Task Consume(ConsumeContext<NewCryptoAdded> context)
        {
            Debug.WriteLine(JsonConvert.SerializeObject(context.Message));
        }
    }
}
