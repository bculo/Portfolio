using Crypto.Application.Interfaces.Services;
using Crypto.Core.Entities;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.UpdatePrice
{
    public class UpdatePriceCommandHandler : IRequestHandler<UpdatePriceCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly ICryptoPriceService _priceService;
        private readonly IPublishEndpoint _publish;
        private readonly IDateTime _time;

        public UpdatePriceCommandHandler(IUnitOfWork work, 
            ICryptoPriceService priceService, 
            IPublishEndpoint publish,
            IDateTime time)
        {
            _work = work;
            _priceService = priceService;
            _publish = publish;
            _time = time;
        }

        public async Task<Unit> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _work.CryptoRepository.FindSingle(i => i.Symbol.ToLower() == request.Symbol.ToLower());

            if (entity is null)
            {
                throw new CryptoCoreException("Item with symbol {0} not found", request.Symbol);
            }

            var priceResponse = await _priceService.GetPriceInfo(request.Symbol);

            if (priceResponse is null)
            {
                throw new CryptoCoreException("Provided symbol not supported");
            }

            var newPriceInstance = new CryptoPrice
            {
                CryptoId = entity.Id,
                Price = priceResponse.Price
            };

            await _work.CryptoPriceRepository.Add(newPriceInstance);
            await _work.Commit();

            await _publish.Publish(new CryptoPriceUpdated
            {
                CreatedOn = _time.DateTime,
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
