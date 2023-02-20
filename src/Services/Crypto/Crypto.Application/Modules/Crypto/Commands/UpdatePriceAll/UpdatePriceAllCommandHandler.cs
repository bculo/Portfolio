using Crypto.Application.Interfaces.Services;
using Crypto.Core.Entities;
using Crypto.Core.Exceptions;
using Crypto.Core.Interfaces;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll
{
    public class UpdatePriceAllCommandHandler : IRequestHandler<UpdatePriceAllCommand>
    {
        private readonly IUnitOfWork _work;
        private readonly ICryptoPriceService _priceService;
        private readonly IPublishEndpoint _publish;
        private readonly IDateTime _time;
        private readonly ILogger<UpdatePriceAllCommandHandler> _logger;

        public UpdatePriceAllCommandHandler(IUnitOfWork work,
            ICryptoPriceService priceService,
            IPublishEndpoint publish,
            IDateTime time,
            ILogger<UpdatePriceAllCommandHandler> logger)
        {
            _work = work;
            _priceService = priceService;
            _publish = publish;
            _time = time;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdatePriceAllCommand request, CancellationToken cancellationToken)
        {
            var entities = await _work.CryptoRepository.GetAll();

            if (!entities.Any())
            {
                _logger.LogTrace("ZERO entities to update");
                return Unit.Value;
            }

            var symbols = entities.Select(i => i.Symbol).ToList();

            var response = await _priceService.GetPriceInfo(symbols!);

            if(response is null)
            {
                throw new CryptoCoreException("Unexcpected error");
            }

            var newPrices = new List<CryptoPrice>();
            var events = new List<CryptoPriceUpdated>();
            foreach(var item in response)
            {
                var crypto = entities.FirstOrDefault(i => i.Symbol!.ToUpper() == item.Symbol.ToUpper());
                
                if(crypto is null)
                {
                    continue;
                }

                newPrices.Add(new CryptoPrice
                {
                    CryptoId = crypto.Id,
                    Price = item.Price
                });

                events.Add(new CryptoPriceUpdated
                {
                    Id = crypto.Id,
                    Currency = item.Currency,
                    Name = crypto.Name,
                    Price = item.Price,
                    Symbol = item.Symbol
                });
            }

            await _work.CryptoPriceRepository.AddRange(newPrices);
            await _work.Commit();

            await PublishEvents(events);

            return Unit.Value;
        }

        private async Task PublishEvents(List<CryptoPriceUpdated> events)
        {
            foreach (var item in events)
            {
                item.CreatedOn = _time.DateTime;
                await _publish.Publish(item);
            }
        }
    }
}
