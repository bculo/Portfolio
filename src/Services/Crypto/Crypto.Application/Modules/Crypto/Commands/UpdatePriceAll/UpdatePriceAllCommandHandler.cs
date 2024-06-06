using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Price.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Time.Abstract.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll
{
    public class UpdatePriceAllCommandHandler(
        IUnitOfWork work,
        ICryptoPriceService priceService,
        IPublishEndpoint publish,
        ILogger<UpdatePriceAllCommandHandler> logger,
        IDateTimeProvider provider)
        : IRequestHandler<UpdatePriceAllCommand, UpdatePriceAllResponse>
    {
        private readonly ILogger<UpdatePriceAllCommandHandler> _logger = logger;

        public async Task<UpdatePriceAllResponse> Handle(UpdatePriceAllCommand request, CancellationToken ct)
        {
            var entities = await work.CryptoRepo.GetAll(ct: ct);
            if (entities.Count == 0)
            {
                return new UpdatePriceAllResponse { NumberOfUpdates = 0 };
            }

            var entityDict = entities.ToDictionary(x => x.Symbol, y => y);
            var symbols = entityDict.Keys.ToList();
            var priceResponses = await priceService.GetPriceInfo(symbols, ct);

            var (prices, events) = GetInstances(priceResponses, entityDict);

            await work.CryptoPriceRepo.BulkInsert(prices, ct);
            await work.Commit(ct);

            await PublishEvents(events);
            
            return new UpdatePriceAllResponse { NumberOfUpdates = prices.Count };
        }

        private (List<CryptoPrice> price, List<CryptoPriceUpdated> events) GetInstances(
            List<PriceResponse> priceResponses,
            Dictionary<string, Core.Entities.Crypto> cryptoDict)
        {
            List<CryptoPrice> prices = new();
            List<CryptoPriceUpdated> events = new();
            foreach(var response in priceResponses)
            {
                if(!cryptoDict.ContainsKey(response.Symbol))
                {
                    continue;
                }
            
                var crypto = cryptoDict[response.Symbol];
                
                prices.Add(new CryptoPrice
                {
                    CryptoId = crypto.Id,
                    Price = response.Price,
                    Time = provider.UtcOffset
                });
                
                events.Add(new CryptoPriceUpdated
                {
                    Id = crypto.Id,
                    Name = crypto.Name,
                    Price = response.Price,
                    Symbol = response.Symbol
                });
            }
            
            return (prices, events);
        }

        private async Task PublishEvents(List<CryptoPriceUpdated> events)
        {
            foreach (var item in events)
            {
                await publish.Publish(item);
            }

            await publish.Publish(new EvictRedisListRequest());
        }
    }
}

