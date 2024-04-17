using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Time.Abstract.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.UpdatePrice
{
    public class UpdatePriceCommandHandler : IRequestHandler<UpdatePriceCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly IDateTimeProvider _timeProvider;
        private readonly ICryptoPriceService _priceService;
        private readonly IPublishEndpoint _publish;

        public UpdatePriceCommandHandler(IUnitOfWork work, 
            ICryptoPriceService priceService, 
            IPublishEndpoint publish, 
            IDateTimeProvider timeProvider)
        {
            _work = work;
            _priceService = priceService;
            _publish = publish;
            _timeProvider = timeProvider;
        }

        public async Task Handle(UpdatePriceCommand request, CancellationToken ct)
        {
            var entity = await _work.CryptoRepo.First(
                i => i.Symbol.ToLower() == request.Symbol.ToLower(), 
                track: true,
                ct: ct);
            
            CryptoCoreNotFoundException.ThrowIfNull(entity, $"Item with symbol {request.Symbol} not found");
            
             var priceResponse = await _priceService.GetPriceInfo(request.Symbol, ct);
             
             var newPriceInstance = new CryptoPrice
             {
                 CryptoId = entity.Id,
                 Price = priceResponse.Price,
                 Time = _timeProvider.UtcOffset
             };

             await _work.CryptoPriceRepo.Add(newPriceInstance, ct);
             await _work.Commit(ct);

             await _publish.Publish(new CryptoPriceUpdated
             {
                 Id = entity.Id,
                 Name = entity.Name,
                 Price = priceResponse.Price,
                 Symbol = entity.Symbol
             }, ct);
        }
    }
}
