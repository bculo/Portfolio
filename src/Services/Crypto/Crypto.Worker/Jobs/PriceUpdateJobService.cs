using Crypto.Application.Interfaces.Repositories;
using Crypto.Worker.Interfaces;
using Events.Common.Crypto;
using MassTransit;
using Time.Abstract.Contracts;

namespace Crypto.Worker.Jobs
{
    public class PriceUpdateJobService : IPriceUpdateJobService
    {
        private readonly IDateTimeProvider _provider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;

        public PriceUpdateJobService(IUnitOfWork work,
            IPublishEndpoint publishEndpoint, 
            IDateTimeProvider provider)
        {
            _unitOfWork = work;
            _publishEndpoint = publishEndpoint;
            _provider = provider;
        }

        public async Task ExecuteUpdate()
        {
            var @event = new UpdateItemsPrices
            {
                Time = _provider.UtcOffset
            };
            
            await _publishEndpoint.Publish(@event);
            await _unitOfWork.Commit(); //Outbox pattern commit
        }
    }
}
