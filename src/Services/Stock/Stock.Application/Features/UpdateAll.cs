using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock.Application.Infrastructure.Persistence;
using Stock.Application.Interfaces;
using Stock.Core.Entities;

namespace Stock.Application.Features
{
    public static class UpdateAll
    {
        public record Command : IRequest { }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IStockPriceClient _client;
            private readonly StockDbContext _dbContext;

            public Handler(StockDbContext dbContext, IStockPriceClient client)
            {
                _dbContext = dbContext;
                _client = client;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var itemsToUpdate = await GetAssetsForPriceUpdate();

                var fetchedItemsWithPrice = await ExecuteUpdateProcedure(itemsToUpdate);

                var priceEntities = await MapToPriceInstances(fetchedItemsWithPrice, itemsToUpdate);

                await Save(priceEntities);
            }

            /// <summary>
            /// Fetch financal assets from database in dictionary form where Key presents Stock symbol and value presents database ID (unique)
            /// </summary>
            /// <returns></returns>
            private async Task<Dictionary<string, int>> GetAssetsForPriceUpdate()
            {
                return await _dbContext.Stocks.Select(i => new DbQuery
                {
                    Id = i.Id,
                    Symbol = i.Symbol,
                }).ToDictionaryAsync(x => x.Symbol, x => x.Id);
            }

            /// <summary>
            /// For each item of dictionary, try to fetch new price
            /// </summary>
            /// <param name="items"></param>
            /// <returns></returns>
            private async Task<List<StockPriceInfo>> ExecuteUpdateProcedure(Dictionary<string, int> items)
            {
                var tasks = new List<StockPriceInfo>();
                foreach (var item in items)
                {
                    tasks.Add(await FetchAssetPrice(item.Key));
                }
                return tasks;

                /*
                var tasks = new List<Task<StockPriceInfo>>();
                foreach (var item in items)
                {
                    tasks.Add(FetchAssetPrice(item.Key));
                }

                await Task.WhenAll(tasks);
                return tasks.Where(i => i.Result is not null)
                    .Select(i => i.Result)
                    .ToList();
                */
            }

            /// <summary>
            /// Fetch price for given symbol using IStockPriceClient
            /// </summary>
            /// <param name="symbol"></param>
            /// <returns></returns>
            private async Task<StockPriceInfo> FetchAssetPrice(string symbol)
            {
                return await _client.GetPriceForSymbol(symbol);
            }

            /// <summary>
            /// Combine fetched prices and stock dictionary to entites of type Price
            /// </summary>
            /// <param name="fetchedItemsWithPrice"></param>
            /// <param name="itemsToUpdate"></param>
            /// <returns></returns>
            private async Task<List<StockPrice>> MapToPriceInstances(List<StockPriceInfo> fetchedItemsWithPrice,
                Dictionary<string, int> itemsToUpdate)
            {
                var priceEntities = new List<StockPrice>();

                foreach (var fetchedItem in fetchedItemsWithPrice)
                {
                    if (!itemsToUpdate.ContainsKey(fetchedItem.Symbol))
                    {
                        continue;
                    }

                    priceEntities.Add(new StockPrice
                    {
                        Price = fetchedItem.Price,
                        StockId = itemsToUpdate[fetchedItem.Symbol]
                    });
                }

                return priceEntities;
            }

            /// <summary>
            /// Persist new price instances to database
            /// </summary>
            /// <param name="priceEntities"></param>
            /// <returns></returns>
            private async Task Save(List<StockPrice> priceEntities)
            {
                _dbContext.Prices.AddRange(priceEntities);
                await _dbContext.SaveChangesAsync();
            }
        }

        private class DbQuery
        {
            public int Id { get; set; }
            public string Symbol { get; set; }
        }
    }
}
