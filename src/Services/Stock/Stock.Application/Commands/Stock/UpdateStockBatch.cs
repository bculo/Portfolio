using Events.Common.Stock;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Price.Models;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;
using Time.Abstract.Contracts;

namespace Stock.Application.Commands.Stock;


public record UpdateStockBatch(List<string> Symbols) : IRequest;

public class UpdateStockBatchValidator : AbstractValidator<UpdateStockBatch>
{
    public UpdateStockBatchValidator()
    {
        RuleFor(i => i.Symbols)
            .NotEmpty();
        
        RuleForEach(i => i.Symbols)
            .NotEmpty();
    }
}

public class UpdateStockBatchHandler : IRequestHandler<UpdateStockBatch>
{
    private readonly IPublishEndpoint _endpoint;
    private readonly IConfiguration _config;
    private readonly IDateTimeProvider _provider;
    private readonly ILogger<UpdateStockBatchHandler> _logger;
    private readonly IStockPriceClient _client;
    private readonly IUnitOfWork _work;

    public UpdateStockBatchHandler(IStockPriceClient client,
        ILogger<UpdateStockBatchHandler> logger,
        IPublishEndpoint endpoint,
        IDateTimeProvider provider,
        IConfiguration config,
        IUnitOfWork work)
    {
        _client = client;
        _logger = logger;
        _endpoint = endpoint;
        _provider = provider;
        _config = config;
        _work = work;
    }

    public async Task Handle(UpdateStockBatch request, CancellationToken cancellationToken)
    {
        var itemsToUpdate = await GetAssetsForPriceUpdate(request.Symbols);

        if (!itemsToUpdate.Any())
        {
            _logger.LogTrace("Zero items for update fetched");
            return;
        }

        var fetchedItemsWithPrice = await FetchNewPricesForSymbols(itemsToUpdate);

        if(!fetchedItemsWithPrice.Any())
        {
            _logger.LogTrace("Items not found via client usage");
            return;
        }

        var priceEntities = MapToPriceInstances(fetchedItemsWithPrice, itemsToUpdate);

        await Save(priceEntities);

        await PublishEvents(fetchedItemsWithPrice);
    }
    
    private async Task PublishEvents(List<StockPriceDetails> itemsWithPrice)
    {
        var timeStamp = _provider.Now;

        foreach (var item in itemsWithPrice)
        {
            await _endpoint.Publish(new StockPriceUpdated
            {
                Id = item.Id,
                Price = item.Price,
                Symbol = item.Symbol,
                UpdatedOn = timeStamp
            });
        }
    }
    
    private async Task<Dictionary<string, int>> GetAssetsForPriceUpdate(List<string> symbols)
    {
        var stocks = await _work.StockRepo.GetAll();
        return stocks.ToDictionary(x => x.Symbol, y => y.Id);
    }
    
    private async Task<List<StockPriceDetails>> FetchNewPricesForSymbols(Dictionary<string, int> items)
    {
        int maximumConcurrentRequest = _config.GetValue<int>("MaximumConcurrentHttpRequests");
        if(maximumConcurrentRequest <= 0)
        {
            _logger.LogWarning("Maximum number of concurrent is less or equal to 0. Check application settings");
            maximumConcurrentRequest = 5;
        }

        using var semaphore = new SemaphoreSlim(maximumConcurrentRequest);
        var tasks = items.Select(async item => await FetchPriceForSingleSymbol(item, semaphore)).ToList();

        var priceInfos = await Task.WhenAll(tasks);
        return priceInfos.ToList();
    }

    public async Task<StockPriceDetails> FetchPriceForSingleSymbol(
        KeyValuePair<string, int> item, 
        SemaphoreSlim semaphore)
    {
        try
        {
            await semaphore.WaitAsync();

            var fetchedInstance = await FetchAssetPrice(item.Key);
            if (fetchedInstance is not null)
            {
                return new StockPriceDetails
                {
                    Id = item.Value,
                    Price = fetchedInstance.Price,
                    Symbol = item.Key
                };
            }

            return default;
        }
        catch
        {
            return default;
        }
        finally
        {
            semaphore.Release();
        }
    }
    
    private async Task<StockPriceInfo> FetchAssetPrice(string symbol)
    {
        return await _client.GetPrice(symbol);
    }
    
    private IEnumerable<StockPriceEntity> MapToPriceInstances(
        List<StockPriceDetails> fetchedItemsWithPrice,
        Dictionary<string, int> itemsToUpdate)
    {
        foreach (var fetchedItem in fetchedItemsWithPrice)
        {
            if (!itemsToUpdate.ContainsKey(fetchedItem.Symbol))
            {
                continue;
            }

            yield return new StockPriceEntity
            {
                Price = fetchedItem.Price,
                StockId = itemsToUpdate[fetchedItem.Symbol]
            };
        }
    }
    
    private async Task Save(IEnumerable<StockPriceEntity> priceEntities)
    {
        await _work.StockPriceRepo.AddRange(priceEntities);
        await _work.Save(default);
    }
    
}

public class StockPriceDetails
{
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public long Id { get; set; }
}


