using Events.Common.Crypto;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Consumers
{
    public class AddCryptoItemFaultConsumer : IConsumer<Fault<AddCryptoItem>>
    {
        public Task Consume(ConsumeContext<Fault<AddCryptoItem>> context)
        {
            return Task.CompletedTask;
        }
    }
}
