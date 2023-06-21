using Events.Common.Stock;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stock.Application.Common.Options;
using Stock.Application.Interfaces;

namespace Stock.Application.Features
{
    /// <summary>
    /// Prepare price update batches for stock items. 
    /// Fetch all stock items and divide them into smaller batches.
    /// Batch size is defiend based on application settings
    /// </summary>
    public static class PrepareBatchesForUpdate
    {
        public record Command : IRequest { }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IPublishEndpoint _endpoint;
            private readonly ILogger<Handler> _logger;
            private readonly IBaseRepository<Core.Entities.Stock> _repo;
            private readonly IOptionsSnapshot<BatchUpdateOptions> _options;

            public Handler(IPublishEndpoint endpoint,
                ILogger<Handler> logger,
                IBaseRepository<Core.Entities.Stock> repo,
                IOptionsSnapshot<BatchUpdateOptions> options)
            {
                _logger = logger;
                _endpoint = endpoint;
                _options = options;
                _repo = repo;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var symbols = await GetStockItems();

                if (!symbols.Any())
                {
                    _logger.LogTrace("Zero symbols found in storage");
                    return;
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
                return await _repo.Filter(i => true, i => i.Symbol);
            }
        }
    }
}
