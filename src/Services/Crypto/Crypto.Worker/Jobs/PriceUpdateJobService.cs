using Crypto.Application.Interfaces.Repositories;
using Crypto.Worker.Interfaces;
using Events.Common.Crypto;
using MassTransit;
using Time.Common;

namespace Crypto.Worker.Jobs
{
    public class PriceUpdateJobService(
        IUnitOfWork work,
        IPublishEndpoint publishEndpoint,
        IDateTimeProvider provider)
        : IPriceUpdateJobService
    {
        public async Task ExecuteUpdate()
        {
            var @event = new UpdateItemsPrices
            {
                Time = provider.TimeOffset
            };
            
            await publishEndpoint.Publish(@event);
            await work.Commit();
        }
    }
}
