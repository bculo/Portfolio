using Events.Common.Stock;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stock.Application.Common.Extensions;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;
using Time.Common;

namespace Stock.Application.Commands.Stock;

public record UpdateStockBatchCommand(List<string> Symbols) : IRequest<UpdateStockBatchResponse>;

public class UpdateStockBatchCommandValidator : AbstractValidator<UpdateStockBatchCommand>
{
    public UpdateStockBatchCommandValidator()
    {
        RuleForEach(i => i.Symbols)
            .NotEmpty();
    }
}

public class UpdateStockBatchHandler(
    IStockPriceClient client,
    IPublishEndpoint endpoint,
    IDateTimeProvider provider,
    IConfiguration config,
    IEntityManagerRepository managerRepository,
    IDataSourceProvider dataProvider)
    : IRequestHandler<UpdateStockBatchCommand, UpdateStockBatchResponse>
{
    private const int MaximumConcurrentRequests = 5;

    public async Task<UpdateStockBatchResponse> Handle(UpdateStockBatchCommand request, CancellationToken ct)
    {
        var assetsReadyForUpdate = await GetStocksForUpdate(request.Symbols, ct);
        if (assetsReadyForUpdate.Count == 0) 
            return new ZeroAssetsForUpdateResponse();

        var assetsWithNewPrices = await FetchSymbolsPrices(assetsReadyForUpdate, ct);
        if(assetsWithNewPrices.Count == 0)
            return new EmptyClientResponse(assetsReadyForUpdate.Count);

        await HandlePriceUpdates(assetsWithNewPrices, assetsReadyForUpdate, ct);
        return new UpdateStockBatchResponse(assetsReadyForUpdate.Count, assetsWithNewPrices.Count);
    }
    
    private async Task PublishPriceUpdatedEvents(List<StockPriceDetails> assets, CancellationToken ct)
    {
        var timeStamp = provider.Time;
        
        foreach (var asset in assets)
        {
            await endpoint.Publish(new StockPriceUpdated
            {
                Id = asset.AssetId,
                Price = asset.Price,
                Symbol = asset.Symbol,
                UpdatedOn = timeStamp
            }, ct);
        }
    }
    
    private async Task<AssetSymbolIdDictionary> GetStocksForUpdate(List<string> symbols, CancellationToken ct)
    {
        var stocks = await dataProvider.GetQuery<StockEntity>()
            .Where(i => symbols.Contains(i.Symbol))
            .ToListAsync(ct);
        
        return AssetSymbolIdDictionary.FromEntities(stocks);
    }
    
    private async Task<List<StockPriceDetails>> FetchSymbolsPrices(
        AssetSymbolIdDictionary assetsReadyForUpdate, 
        CancellationToken ct)
    {
        var maximumConcurrentRequest =
            config.GetValue<int?>("MaximumConcurrentHttpRequests") ?? MaximumConcurrentRequests;
        
        using var semaphore = new SemaphoreSlim(maximumConcurrentRequest);
        var fetchPriceTasks = assetsReadyForUpdate
            .Select(item => FetchSymbolPriceBySymbol(item, semaphore, ct))
            .ToList();

        var newPrices = await Task.WhenAll(fetchPriceTasks);
        return newPrices.RemoveNulls();
    }

    private async Task<StockPriceDetails?> FetchSymbolPriceBySymbol(StockSymbolIdValue item, SemaphoreSlim semaphore,
        CancellationToken ct)
    {
        try
        {
            await semaphore.WaitAsync(ct);
            var clientResponse = await client.GetPrice(item.Symbol, ct);
            return clientResponse != null
                ? new StockPriceDetails(item.Symbol, clientResponse.Price, item.AssetId)
                : null;
        }
        catch
        {
            return null;
        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task HandlePriceUpdates(List<StockPriceDetails> fetchedItemsWithPrice,
        AssetSymbolIdDictionary itemsToUpdate, CancellationToken ct)
    {
        List<StockPriceEntity> newPrices = [];
        
        foreach (var fetchedItem in fetchedItemsWithPrice)
        {
            var assetReadyForUpdate = itemsToUpdate.FindBySymbol(fetchedItem.Symbol);
            
            if(assetReadyForUpdate == null)
                continue;
            
            newPrices.Add(new StockPriceEntity
            {
                IsActive = true,
                Price = fetchedItem.Price,
                StockId = assetReadyForUpdate.AssetId
            });
        }

        await managerRepository.AddRange(newPrices, ct);
        await PublishPriceUpdatedEvents(fetchedItemsWithPrice, ct);
    }
}


internal class AssetSymbolIdDictionary : Dictionary<string, Guid>
{
    public StockSymbolIdValue? FindBySymbol(string symbol)
    {
        return !TryGetValue(symbol, out var id) ? null : new StockSymbolIdValue(symbol, id);
    }

    public static AssetSymbolIdDictionary FromEntities(List<StockEntity> entities)
    {
        var dict = new AssetSymbolIdDictionary();
        foreach (var entity in entities)
        {
            dict[entity.Symbol] = entity.Id;
        }
        return dict;
    }
}
    
internal record StockSymbolIdValue(string Symbol, Guid AssetId)
{
    public static implicit operator StockSymbolIdValue(KeyValuePair<string, Guid> d) => new (d.Key, d.Value);    
}

internal record StockPriceDetails(string Symbol, decimal Price, Guid AssetId);

public record UpdateStockBatchResponse(int AssetsToUpdate, int UpdatedAssets);
public record ZeroAssetsForUpdateResponse() : UpdateStockBatchResponse(0, 0);
public record EmptyClientResponse(int AssetsToUpdate) : UpdateStockBatchResponse(AssetsToUpdate, 0);

