using Events.Common.Stock;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stock.Application.Common.Options;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;
using Time.Common;

namespace Stock.Application.Commands.Stock;

public record PrepareStockUpdateBatches : IRequest<PrepareStockUpdateBatchesResult>;

public class PrepareStockUpdateBatchesHandler(
    IDataSourceProvider sourceProvider,
    IPublishEndpoint endpoint,
    ILogger<PrepareStockUpdateBatchesHandler> logger,
    IOptionsSnapshot<BatchUpdateOptions> options,
    IDateTimeProvider timeProvider)
    : IRequestHandler<PrepareStockUpdateBatches, PrepareStockUpdateBatchesResult>
{
    private const int DefaultBatchSize = 5;

    public async Task<PrepareStockUpdateBatchesResult> Handle(
        PrepareStockUpdateBatches request, 
        CancellationToken ct)
    {
        if (!IsUsStockExchangeActive())
            return PrepareStockUpdateBatchesResult.Empty();
        
        var symbols = await FetchAssetsReadyForUpdate(ct);
        if (!symbols.Any())
            return PrepareStockUpdateBatchesResult.Empty();

        var batches = CreateBatches(symbols);
        
        await PublishBatches(batches, ct);
        
        return PrepareStockUpdateBatchesResult.WithBatches(batches.Count);
    }

    private bool IsUsStockExchangeActive()
    {
        var utcTime = timeProvider.Time;

        if (!options.Value.IgnoreExchangeActiveTime)
            return false;
        
        if (utcTime.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            return false;
        
        var timeOfDayUtc = utcTime.TimeOfDay;
        var usExchangeOpenUtc = new TimeSpan(14, 30, 0);
        var usExchangeCloseUtc = new TimeSpan(21, 0, 0);
        return timeOfDayUtc >= usExchangeOpenUtc && timeOfDayUtc <= usExchangeCloseUtc;
    }

    private async Task PublishBatches(Dictionary<int, List<string>> batches, CancellationToken ct)
    {
        foreach (var (_, symbols) in batches)
        {
            await endpoint.Publish(new UpdateBatchPrepared
            {
                Symbols = symbols
            }, ct);
        }
    }

    private Dictionary<int, List<string>> CreateBatches(List<string> symbols)
    {
        var batchSize = options.Value.BatchSize;
        if (batchSize <= 0)
        {
            logger.LogWarning("BatchSize option is less or equal to zero. Check application settings!");
            batchSize = DefaultBatchSize;
        }

        var batches = new Dictionary<int, List<string>>();
        var numberOfBatches = (int)Math.Ceiling((decimal)symbols.Count / batchSize);
        foreach (var batchIndex in Enumerable.Range(0, numberOfBatches))
        {
            var batchItems = symbols.Skip(batchIndex * batchSize)
                .Take(batchSize)
                .ToList();
            
            batches.Add(batchIndex, batchItems);
        }

        return batches;
    }

    private async Task<List<string>> FetchAssetsReadyForUpdate(CancellationToken ct)
    {
        return await sourceProvider.GetQuery<StockEntity>()
            .Where(i => i.IsActive)
            .Select(i => i.Symbol)
            .ToListAsync(ct);
    }
}

public record PrepareStockUpdateBatchesResult(int NumberOfBatches)
{
    public static PrepareStockUpdateBatchesResult Empty() => new(0);
    public static PrepareStockUpdateBatchesResult WithBatches(int numberOfBatches) => new(numberOfBatches);
}
    