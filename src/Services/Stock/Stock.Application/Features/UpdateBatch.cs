﻿using Events.Common.Stock;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces;
using Stock.Core.Entities;
using Time.Abstract.Contracts;

namespace Stock.Application.Features
{
    public static class UpdateBatch
    {
        public record Command : IRequest 
        {
            public List<string> Symbols { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(i => i.Symbols).NotNull().NotEmpty();
                RuleForEach(i => i.Symbols).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IPublishEndpoint _endpoint;
            private readonly IDateTimeProvider _provider;
            private readonly ILogger<Handler> _logger;
            private readonly IStockPriceClient _client;
            private readonly IBaseRepository<StockPrice> _repoPrice;
            private readonly IBaseRepository<Core.Entities.Stock> _repoStock;

            public Handler(IStockPriceClient client,
                ILogger<Handler> logger,
                IPublishEndpoint endpoint,
                IDateTimeProvider provider,
                IBaseRepository<StockPrice> repoPrice,
                IBaseRepository<Core.Entities.Stock> repoStock)
            {
                _client = client;
                _logger = logger;
                _endpoint = endpoint;
                _provider = provider;
                _repoPrice = repoPrice;
                _repoStock = repoStock;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var itemsToUpdate = await GetAssetsForPriceUpdate(request.Symbols);

                if (!itemsToUpdate.Any())
                {
                    _logger.LogTrace("Zero items for update fetched");
                    return;
                }

                var fetchedItemsWithPrice = await ExecuteUpdateProcedure(itemsToUpdate);

                if(!fetchedItemsWithPrice.Any())
                {
                    _logger.LogTrace("Items not found via client usage");
                    return;
                }

                var priceEntities = MapToPriceInstances(fetchedItemsWithPrice, itemsToUpdate);

                await Save(priceEntities);

                await PublishEvents(fetchedItemsWithPrice);
            }

            /// <summary>
            /// Publish events to message broker
            /// </summary>
            /// <param name="priceEntities"></param>
            /// <returns></returns>
            private async Task PublishEvents(IEnumerable<StockPriceInfo> itemsWithPrice)
            {
                var timeStamp = _provider.Now;

                foreach (var item in itemsWithPrice)
                {
                    await _endpoint.Publish(new StockPriceUpdated
                    {
                        Price = item.Price,
                        Symbol = item.Symbol,
                        UpdatedOn = timeStamp
                    });
                }
            }

            /// <summary>
            /// Fetch financal assets from database in dictionary form where Key presents Stock symbol and value presents database ID (unique)
            /// </summary>
            /// <returns></returns>
            private async Task<Dictionary<string, int>> GetAssetsForPriceUpdate(List<string> symbols)
            {
                return await _repoStock.GetDictionary(i => symbols.Contains(i.Symbol), x => x.Symbol, y => y.Id);
            }

            /// <summary>
            /// For each item of dictionary, try to fetch new price
            /// </summary>
            /// <param name="items"></param>
            /// <returns></returns>
            private async Task<IEnumerable<StockPriceInfo>> ExecuteUpdateProcedure(Dictionary<string, int> items)
            {
                var priceInfos = new List<StockPriceInfo>();

                foreach (var item in items)
                {
                    var priceInfoItem = await FetchAssetPrice(item.Key);
                    if(priceInfoItem is not null)
                    {
                        priceInfos.Add(priceInfoItem);
                    }
                }

                return priceInfos;
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
            private IEnumerable<StockPrice> MapToPriceInstances(IEnumerable<StockPriceInfo> fetchedItemsWithPrice,
                Dictionary<string, int> itemsToUpdate)
            {
                foreach (var fetchedItem in fetchedItemsWithPrice)
                {
                    if (!itemsToUpdate.ContainsKey(fetchedItem.Symbol))
                    {
                        continue;
                    }

                    yield return new StockPrice
                    {
                        Price = fetchedItem.Price,
                        StockId = itemsToUpdate[fetchedItem.Symbol]
                    };
                }
            }

            /// <summary>
            /// Persist new price instances to database
            /// </summary>
            /// <param name="priceEntities"></param>
            /// <returns></returns>
            private async Task Save(IEnumerable<StockPrice> priceEntities)
            {
                await _repoPrice.Add(priceEntities.ToArray());
                await _repoPrice.SaveChanges();
            }
        }
    }
}