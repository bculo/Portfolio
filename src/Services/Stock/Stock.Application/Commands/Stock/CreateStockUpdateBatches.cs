using Events.Common.Stock;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stock.Application.Common.Options;
using Time.Common;

namespace Stock.Application.Commands.Stock;

public record CreateStockUpdateBatches : IRequest<CreateStockUpdateBatchesResponse>;

public class CreateStockUpdateBatchesHandler(
    IPublishEndpoint endpoint,
    ILogger<CreateStockUpdateBatchesHandler> logger,
    IOptionsSnapshot<BatchUpdateOptions> options,
    IDateTimeProvider timeProvider)
    : IRequestHandler<CreateStockUpdateBatches, CreateStockUpdateBatchesResponse>
{
    private const int TimeDifference = 3;
    private const int DefaultBatchSize = 5;

    public async Task<CreateStockUpdateBatchesResponse> Handle(
        CreateStockUpdateBatches request, 
        CancellationToken ct)
    {
        if (!options.Value.IgnoreExchangeActiveTime && !IsUsStockExchangeActive())
        {
            logger.LogTrace("US stock exchange is closed");
            return new CreateStockUpdateBatchesResponse(0);
        }
        
        var symbols = await GetSymbols();
        if (!symbols.Any())
        {
            logger.LogWarning("Stock symbols not found");
            return new CreateStockUpdateBatchesResponse(0);
        }

        var batches = CreateBatches(symbols);
        logger.LogTrace("Num of batches {NumberOfBatches}, prepared for update", batches.Count);

        await PublishBatches(batches, ct);
        return new CreateStockUpdateBatchesResponse(batches.Count);
    }

    private bool IsUsStockExchangeActive()
    {
        var utcTime = timeProvider.Utc;
        if (utcTime.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            return false;
        }
        var timeOfDayUtc = utcTime.TimeOfDay;
        var usExchangeOpenUtc = new TimeSpan(14, 30, 0);
        var usExchangeCloseUtc = new TimeSpan(21, 0, 0);
        return timeOfDayUtc >= usExchangeOpenUtc && timeOfDayUtc <= usExchangeCloseUtc;
    }

    private async Task PublishBatches(Dictionary<int, List<string>> batches,
        CancellationToken ct)
    {
        foreach (var (_, symbols) in batches)
        {
            await endpoint.Publish(new UpdateBatchPrepared
            {
                Symbols = symbols
            }, ct);
        }

        await work.Save(ct);
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

    private async Task<List<string>> GetSymbols()
    {
        var stockItems = await work.StockRepo.Filter(i => i.IsActive == true);
        
        return stockItems
            .Select(i => i.Symbol)
            .ToList();
    }
}

public record CreateStockUpdateBatchesResponse(int NumberOfBatches);
    