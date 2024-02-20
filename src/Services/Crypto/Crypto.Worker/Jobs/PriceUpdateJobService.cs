using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Exceptions;
using Crypto.Worker.Interfaces;
using Events.Common.Crypto;
using MassTransit;

namespace Crypto.Worker.Jobs
{
    public class PriceUpdateJobService : IPriceUpdateJobService
    {
        private readonly ILogger<PriceUpdateJobService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendEndpointProvider _endpointProvider;

        public PriceUpdateJobService(ILogger<PriceUpdateJobService> logger,
            IUnitOfWork work,
            ISendEndpointProvider provider)
        {
            _logger = logger;
            _unitOfWork = work;
            _endpointProvider = provider;
        }

        public async Task ExecuteUpdate()
        {
            var endpoint = await _endpointProvider.GetSendEndpoint(new Uri($"queue:crypto-update-crypto-items-price"));

            if (endpoint is null)
            {
                throw new CryptoCoreNotFoundException($"Message broker endpoint crypto-update-crypto-items-price not found");
            }

            await endpoint.Send(new UpdateCryptoItemsPrice { });
            await _unitOfWork.Commit(); //Outbox pattern commit
        }
    }
}
