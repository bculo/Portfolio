using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Price.Models;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using Events.Common.Crypto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Time.Common;

namespace Crypto.Application.Modules.Crypto.Commands;

public record UpdatePriceAllCommand : IRequest<UpdatePriceAllResponse>;
public record UpdatePriceAllResponse(int NumberOfUpdates);

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
            return new UpdatePriceAllResponse(0);
        }

        var entityDict = entities.ToDictionary(x => x.Symbol, y => y);
        var symbols = entityDict.Keys.ToList();
        var priceResponses = await priceService.GetPriceInfo(symbols, ct);

        var (prices, events) = GetInstances(priceResponses, entityDict);

        await work.CryptoPriceRepo.BulkInsert(prices, ct);
        await work.Commit(ct);

        await PublishEvents(events);
        
        return new UpdatePriceAllResponse(prices.Count);
    }

    private (List<CryptoPriceEntity> price, List<CryptoPriceUpdated> events) GetInstances(
        List<PriceResponse> priceResponses,
        Dictionary<string, CryptoEntity> cryptoDict)
    {
        List<CryptoPriceEntity> prices = [];
        List<CryptoPriceUpdated> events = [];
        foreach(var response in priceResponses)
        {
            if(!cryptoDict.ContainsKey(response.Symbol))
            {
                continue;
            }
        
            var crypto = cryptoDict[response.Symbol];
            
            prices.Add(new CryptoPriceEntity
            {
                CryptoEntityId = crypto.Id,
                Price = response.Price,
                Time = provider.TimeOffset
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