using Crypto.Application.Interfaces.Persistence;
using Crypto.Application.Interfaces.Services;
using Crypto.Core.Entities;
using Crypto.Core.Exceptions;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Time.Common.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll
{
    public class UpdatePriceAllCommandHandler : IRequestHandler<UpdatePriceAllCommand>
    {
        private readonly ICryptoDbContext _work;
        private readonly ICryptoPriceService _priceService;
        private readonly IPublishEndpoint _publish;
        private readonly IDateTime _time;
        private readonly ILogger<UpdatePriceAllCommandHandler> _logger;

        public UpdatePriceAllCommandHandler(ICryptoDbContext work,
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
            var entityDict = await _work.Cryptos.AsNoTracking().ToDictionaryAsync(x => x.Symbol!, y => y);

            if (!entityDict.Any())
            {
                _logger.LogTrace("ZERO entities to update");
                return Unit.Value;
            }

            var symbols = entityDict.Keys.ToList();

            var response = await _priceService.GetPriceInfo(symbols!);

            if(response is null)
            {
                throw new CryptoCoreException("Unexcpected error");
            }

            var newPrices = new List<CryptoPrice>();
            var events = new List<CryptoPriceUpdated>();

            foreach(var item in response)
            {
                if(!entityDict.ContainsKey(item.Symbol!.ToUpper()))
                {
                    continue;
                }

                var crypto = entityDict[item.Symbol!.ToUpper()];

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

            await _work.SaveChangesAsync(cancellationToken);

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
