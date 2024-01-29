using Events.Common.Stock;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stock.Application.Common.Options;
using Stock.Application.Interfaces.Repositories;

namespace Stock.Application.Commands.Stock;

public record CreateStockUpdateBatches : IRequest;

public class CreateStockUpdateBatchesHandler : IRequestHandler<CreateStockUpdateBatches>
{
    private readonly IPublishEndpoint _endpoint;
    private readonly ILogger<CreateStockUpdateBatchesHandler> _logger;
    private readonly IUnitOfWork _work;
    private readonly IOptionsSnapshot<BatchUpdateOptions> _options;

    public CreateStockUpdateBatchesHandler(IPublishEndpoint endpoint,
        ILogger<CreateStockUpdateBatchesHandler> logger,
        IUnitOfWork work,
        IOptionsSnapshot<BatchUpdateOptions> options)
    {
        _logger = logger;
        _endpoint = endpoint;
        _options = options;
        _work = work;
    }

    public async Task Handle(CreateStockUpdateBatches request, CancellationToken ct)
    {
        var symbols = await GetStockItems();

        if (!symbols.Any())
        {
            _logger.LogTrace("Zero symbols found in storage");
        }

        var batches = CreateBatches(symbols);
        _logger.LogTrace("{NumberOfBatches} prepared for update", batches.Count);

        foreach (var batch in batches)
        {
            await _endpoint.Publish(new BatchForUpdatePrepared
            {
                Symbols = batch
            });
        }
    }

    private List<List<string>> CreateBatches(List<string> symbols)
    {
        int batchSize = _options.Value.BatchSize;
        if (batchSize <= 0)
        {
            _logger.LogWarning("BatchSize option is less or equal to zero. Check application settings!!!");
            batchSize = 10;
        }

        var batches = new List<List<string>>();
        var numberOfBatches = (int)Math.Ceiling((decimal)symbols.Count / batchSize);
        foreach (var batchIndex in Enumerable.Range(0, numberOfBatches))
        {
            var singleBatchItems = symbols.Skip(batchIndex * batchSize)
                .Take(batchSize)
                .ToList();
            batches.Add(singleBatchItems);
        }

        return batches;
    }

    private async Task<List<string>> GetStockItems()
    {
        return (await _work.StockRepo.Filter(i => true)).Select(i => i.Symbol).ToList();
    }
}
    