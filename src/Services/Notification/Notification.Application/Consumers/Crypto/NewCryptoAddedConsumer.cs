using Events.Common.Crypto;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Consumers.Crypto
{
    public class NewCryptoAddedConsumer : IConsumer<NewCryptoAdded>
    {
        public NewCryptoAddedConsumer()
        {

        }

        public Task Consume(ConsumeContext<NewCryptoAdded> context)
        {
            throw new NotImplementedException();
        }
    }
}
