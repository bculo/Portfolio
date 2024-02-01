using Events.Common.Stock;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sqids;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;
using Time.Abstract.Contracts;

namespace Stock.Application.Commands.Stock;

public record UpdateStockBatch(List<string> Symbols) : IRequest<UpdateStockBatchResponse>;

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

public class UpdateStockBatchHandler : IRequestHandler<UpdateStockBatch, UpdateStockBatchResponse>
{
    private const int MAXIMUM_CONCURRENT_REQUESTS = 5;
    
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

    public async Task<UpdateStockBatchResponse> Handle(UpdateStockBatch request, CancellationToken ct)
    {
        var assetsToUpdate = await GetStocksForUpdate(request.Symbols);
        if (!assetsToUpdate.Any())
        {
            _logger.LogWarning("Given batch is empty");
            return new UpdateStockBatchResponse(0, 0);
        }

        var assetsWithFreshPriceTag = await FetchSymbolsPrices(assetsToUpdate);
        if(!assetsWithFreshPriceTag.Any())
        {
            _logger.LogWarning("Items not found via client usage");
            return new UpdateStockBatchResponse(assetsToUpdate.Count, 0);
        }

        var priceEntities = ToEntities(assetsWithFreshPriceTag, assetsToUpdate);
        await _work.StockPriceRepo.AddRange(priceEntities, ct);
        await _work.Save(ct);

        await PublishEvents(assetsWithFreshPriceTag, ct);
        
        return new UpdateStockBatchResponse(assetsToUpdate.Count, assetsWithFreshPriceTag.Count);
    }
    
    private async Task PublishEvents(List<StockPriceDetails> assets, CancellationToken ct)
    {
        var timeStamp = _provider.Now;
        foreach (var asset in assets)
        {
            await _endpoint.Publish(new PriceUpdated
            {
                Id = asset.Id,
                Price = asset.Price,
                Symbol = asset.Symbol,
                UpdatedOn = timeStamp
            }, ct);
        }
    }
    
    private async Task<Dictionary<string, int>> GetStocksForUpdate(List<string> symbols)
    {
        var stocks = await _work.StockRepo.Filter(i => symbols.Contains(i.Symbol));
        return stocks.ToDictionary(x => x.Symbol, y => y.Id);
    }
    
    private async Task<List<StockPriceDetails>> FetchSymbolsPrices(Dictionary<string, int> items)
    {
        var maximumConcurrentRequest = _config.GetValue<int>("MaximumConcurrentHttpRequests");
        if(maximumConcurrentRequest <= 0)
        {
            _logger.LogWarning("Maximum number of concurrent is less or equal to 0. Check application settings");
            maximumConcurrentRequest = MAXIMUM_CONCURRENT_REQUESTS;
        }

        using var semaphore = new SemaphoreSlim(maximumConcurrentRequest);
        var tasks = items.Select(item => FetchSymbolPrice(item, semaphore)).ToList();

        var priceInfos = await Task.WhenAll(tasks);
        return priceInfos.ToList();
    }

    private async Task<StockPriceDetails> FetchSymbolPrice(
        KeyValuePair<string, int> item, 
        SemaphoreSlim semaphore)
    {
        try
        {
            await semaphore.WaitAsync();
            var fetchedInstance = await _client.GetPrice(item.Key);
            return fetchedInstance is not null
                ? new StockPriceDetails(Id: item.Value, Price: fetchedInstance.Price, Symbol: item.Key)
                : default;
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
    
    private IEnumerable<StockPriceEntity> ToEntities(
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
                StockId = itemsToUpdate[fetchedItem.Symbol],
                IsActive = true
            };
        }
    }
}

public record StockPriceDetails(string Symbol, decimal Price, int Id);

public record UpdateStockBatchResponse(int BatchSize, int PriceUpdates);


