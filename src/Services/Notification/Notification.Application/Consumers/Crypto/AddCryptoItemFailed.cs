using Events.Common.Crypto;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Consumers.Crypto
{
    public class AddCryptoItemFailedConsumer : IConsumer<AddCryptoItemFailed>
    {
        public AddCryptoItemFailedConsumer()
        {

        }

        public Task Consume(ConsumeContext<AddCryptoItemFailed> context)
        {
            throw new NotImplementedException();
        }
    }
}
