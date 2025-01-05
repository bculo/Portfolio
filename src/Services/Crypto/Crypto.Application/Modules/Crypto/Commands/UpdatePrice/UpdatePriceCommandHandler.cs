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
    public class UpdatePriceCommandHandler(
        IUnitOfWork work,
        ICryptoPriceService priceService,
        IPublishEndpoint publish,
        IDateTimeProvider timeProvider)
        : IRequestHandler<UpdatePriceCommand>
    {
        public async Task Handle(UpdatePriceCommand request, CancellationToken ct)
        {
            var entity = await work.CryptoRepo.First(
                i => i.Symbol.ToLower() == request.Symbol.ToLower(), 
                track: true,
                ct: ct);

            if (entity == null)
            {
                throw new CryptoCoreNotFoundException($"Item with symbol {request.Symbol} not found");
            }
            
             var priceResponse = await priceService.GetPriceInfo(request.Symbol, ct);
             
             var newPriceInstance = new CryptoPriceEntity
             {
                 CryptoEntityId = entity.Id,
                 Price = priceResponse.Price,
                 Time = timeProvider.UtcOffset
             };

             await work.CryptoPriceRepo.Add(newPriceInstance, ct);
             await work.Commit(ct);

             await publish.Publish(new CryptoPriceUpdated
             {
                 Id = entity.Id,
                 Name = entity.Name,
                 Price = priceResponse.Price,
                 Symbol = entity.Symbol
             }, ct);
        }
    }
}
