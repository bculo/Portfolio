using Events.Common.Stock;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stock.Application.Common.Extensions;
using Stock.Application.Common.Options;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;
using Time.Abstract.Contracts;

namespace Stock.Application.Commands.Stock;

public record CreateStockUpdateBatches : IRequest<CreateStockUpdateBatchesResponse>;

public class CreateStockUpdateBatchesHandler 
    : IRequestHandler<CreateStockUpdateBatches, CreateStockUpdateBatchesResponse>
{
    private const int TIME_DIFFERENCE = 3;
    private const int DEFAULT_BATCH_SIZE = 5;
    
    private readonly IPublishEndpoint _endpoint;
    private readonly ILogger<CreateStockUpdateBatchesHandler> _logger;
    private readonly IUnitOfWork _work;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IOptionsSnapshot<BatchUpdateOptions> _options;

    public CreateStockUpdateBatchesHandler(IPublishEndpoint endpoint,
        ILogger<CreateStockUpdateBatchesHandler> logger,
        IUnitOfWork work,
        IOptionsSnapshot<BatchUpdateOptions> options, 
        IDateTimeProvider timeProvider)
    {
        _logger = logger;
        _endpoint = endpoint;
        _options = options;
        _timeProvider = timeProvider;
        _work = work;
    }

    public async Task<CreateStockUpdateBatchesResponse> Handle(
        CreateStockUpdateBatches request, 
        CancellationToken ct)
    {
        if (!IsUsStockExchangeActive())
        {
            _logger.LogTrace("US stock exchange is closed");
            return new CreateStockUpdateBatchesResponse(0);
        }
        
        var symbols = await GetSymbols();
        if (!symbols.Any())
        {
            _logger.LogWarning("Stock symbols not found");
            return new CreateStockUpdateBatchesResponse(0);
        }

        var priceIteration = await GetNextPriceIteration(ct);

        var batches = CreateBatches(symbols);
        _logger.LogTrace("Num of batches {NumberOfBatches}, prepared for update", batches.Count);

        await PublishBatches(batches, priceIteration, ct);
        return new CreateStockUpdateBatchesResponse(batches.Count);
    }

    private async Task<StockPriceIterationEntity> GetNextPriceIteration(CancellationToken ct)
    {
        var latestIteration = await _work.PriceIteration.First(
            i => true,
            orderBy: i => i.OrderByDescending(x => x.Time),
            ct: ct);
        
        var currentTime = _timeProvider.Utc;
        var iteration = new StockPriceIterationEntity
        {
            Time = DependsOnLastIteration(latestIteration, currentTime)
                        ? latestIteration!.Time.AddMinutes(TIME_DIFFERENCE)
                        : currentTime.WithoutSeconds()
        };
        
        await _work.PriceIteration.Add(iteration, ct);
        await _work.Save(ct);

        return iteration;
    }

    private bool DependsOnLastIteration(StockPriceIterationEntity? lastIteration, DateTime currentTime)
    {
        if (lastIteration is null || lastIteration.Time.Day != currentTime.Day)
        {
            return false;
        }

        return true;
    }

    private bool IsUsStockExchangeActive()
    {
        var timeOfDayUtc = _timeProvider.Utc.TimeOfDay;
        var usExchangeOpenUtc = new TimeSpan(14, 30, 0);
        var usExchangeCloseUtc = new TimeSpan(21, 0, 0);
        return timeOfDayUtc >= usExchangeOpenUtc && timeOfDayUtc <= usExchangeCloseUtc;
    }

    private async Task PublishBatches(Dictionary<int, List<string>> batches, 
        StockPriceIterationEntity iteration, 
        CancellationToken ct)
    {
        foreach (var (_, symbols) in batches)
        {
            await _endpoint.Publish(new UpdateBatchPrepared
            {
                IterationId = iteration.Id,
                Symbols = symbols
            }, ct);
        }
    }

    private Dictionary<int, List<string>> CreateBatches(List<string> symbols)
    {
        var batchSize = _options.Value.BatchSize;
        if (batchSize <= 0)
        {
            _logger.LogWarning("BatchSize option is less or equal to zero. Check application settings!");
            batchSize = DEFAULT_BATCH_SIZE;
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
        var stockItems = await _work.StockRepo.Filter(i => i.IsActive == true);
        
        return stockItems
            .Select(i => i.Symbol)
            .ToList();
    }
}

public record CreateStockUpdateBatchesResponse(int NumberOfBatches);
    