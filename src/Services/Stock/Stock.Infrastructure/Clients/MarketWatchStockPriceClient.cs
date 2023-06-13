﻿using BultInTypes.Common.Decimal;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Constants;
using Stock.Application.Interfaces;
using Time.Abstract.Contracts;

namespace Stock.Infrastructure.Clients
{
    public class MarketWatchStockPriceClient : IStockPriceClient
    {
        private readonly IHtmlParser _htmlParser;
        private readonly IHttpClientFactory _factory;
        private readonly IDateTimeProvider _timeProvider;
        private readonly ILogger<MarketWatchStockPriceClient> _logger;

        public MarketWatchStockPriceClient(IHtmlParser htmlParser,
            ILogger<MarketWatchStockPriceClient> logger,
            IHttpClientFactory factory,
            IDateTimeProvider timeProvider)
        {
            _logger = logger;
            _factory = factory;
            _htmlParser = htmlParser;
            _timeProvider = timeProvider;
        }

        public async Task<StockPriceInfo> GetPriceForSymbol(string symbol)
        {
            ArgumentException.ThrowIfNullOrEmpty(symbol);

            var client = _factory.CreateClient(HttpClientNames.MARKET_WATCH);

            var response = await client.GetAsync($"investing/stock/{symbol}?mod=mw_quote_tab");
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var htmlAsString = await response.Content.ReadAsStringAsync();
            await _htmlParser.InitializeHtmlContent(htmlAsString);

            var priceSectionNode = await _htmlParser.FindSingleElement("//div[@class='intraday__data']/h2/bg-quote");
            if (priceSectionNode is null)
            {
                _logger.LogWarning("Node for given XPath not found");
                return default;
            }

            var conversionResult = priceSectionNode.Text.ToNullableDecimal();
            if (!conversionResult.HasValue)
            {
                _logger.LogWarning("Problem occured when parsing string to decimal number. String is {0}", priceSectionNode.Text);
                return default;
            }

            return new StockPriceInfo
            {
                Price = conversionResult.Value,
                Symbol = symbol,
                FetchedTimestamp = _timeProvider.Now
            };
        }
    }
}