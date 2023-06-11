using Crypto.Application.Interfaces.Services;
using Crypto.Core.Entities;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using Events.Common.Crypto;
using MassTransit;
using MediatR;

namespace Crypto.Application.Modules.Crypto.Commands.UpdatePrice
{
    public class UpdatePriceCommandHandler : IRequestHandler<UpdatePriceCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly ICryptoPriceService _priceService;
        private readonly IPublishEndpoint _publish;

        public UpdatePriceCommandHandler(IUnitOfWork work, 
            ICryptoPriceService priceService, 
            IPublishEndpoint publish)
        {
            _work = work;
            _priceService = priceService;
            _publish = publish;
        }

        public async Task<Unit> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _work.CryptoRepository.FindSingle(i => i.Symbol!.ToLower() == request.Symbol!.ToLower());
            CryptoCoreException.ThrowIfNull(entity, $"Item with symbol {request.Symbol} not found");

            var priceResponse = await _priceService.GetPriceInfo(request.Symbol);
            CryptoCoreException.ThrowIfNull(priceResponse, "Provided symbol not supported");

            var newPriceInstance = new CryptoPrice
            {
                CryptoId = entity.Id,
                Price = priceResponse.Price
            };

            await _work.Commit();
            await _publish.Publish(new CryptoPriceUpdated
            {
                Currency = priceResponse.Currency,
                Id = entity.Id,
                Name = entity.Name,
                Price = priceResponse.Price,
                Symbol = entity.Symbol
            });

            return Unit.Value;
        }
    }
}
