﻿using Crypto.Application.Interfaces.Price;
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
    public class UpdatePriceAllCommandHandler : IRequestHandler<UpdatePriceAllCommand, UpdatePriceAllResponse>
    {
        private readonly IUnitOfWork _work;
        private readonly ICryptoPriceService _priceService;
        private readonly IPublishEndpoint _publish;
        private readonly IDateTimeProvider _provider;
        private readonly ILogger<UpdatePriceAllCommandHandler> _logger;

        public UpdatePriceAllCommandHandler(IUnitOfWork work,
            ICryptoPriceService priceService,
            IPublishEndpoint publish,
            ILogger<UpdatePriceAllCommandHandler> logger, 
            IDateTimeProvider provider)
        {
            _work = work;
            _priceService = priceService;
            _publish = publish;
            _logger = logger;
            _provider = provider;
        }

        public async Task<UpdatePriceAllResponse> Handle(UpdatePriceAllCommand request, CancellationToken ct)
        {
            var entities = await _work.CryptoRepo.GetAll(ct: ct);
            if (!entities.Any())
            {
                return new UpdatePriceAllResponse { NumberOfUpdates = 0 };
            }

            var entityDict = entities.ToDictionary(x => x.Symbol, y => y);
            var symbols = entityDict.Keys.ToList();
            var priceResponses = await _priceService.GetPriceInfo(symbols, ct);

            var (prices, events) = GetInstances(priceResponses, entityDict);

            await _work.CryptoPriceRepo.BulkInsert(prices, ct);

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
                    Time = _provider.UtcOffset
                });
                
                events.Add(new CryptoPriceUpdated
                {
                    Id = crypto.Id,
                    Currency = response.Currency,
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
                await _publish.Publish(item);
            }
        }
    }
}

