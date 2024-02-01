using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrimitiveTypes.Common.Decimal;
using Stock.Application.Common.Configurations;
using Stock.Application.Interfaces.Html;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Price.Models;
using Time.Abstract.Contracts;

namespace Stock.Infrastructure.Price
{
    public class MarketWatchStockPriceClient : IStockPriceClient
    {
        private readonly IServiceProvider _provider;
        private readonly IHttpClientFactory _factory;
        private readonly IDateTimeProvider _timeProvider;
        private readonly ILogger<MarketWatchStockPriceClient> _logger;

        public MarketWatchStockPriceClient(IServiceProvider provider,
            ILogger<MarketWatchStockPriceClient> logger,
            IHttpClientFactory factory,
            IDateTimeProvider timeProvider)
        {
            _logger = logger;
            _factory = factory;
            _provider = provider;
            _timeProvider = timeProvider;
        }

        public async Task<StockPriceInfo?> GetPrice(string symbol, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(symbol);

            var client = _factory.CreateClient(HttpClientNames.MARKET_WATCH);
            var response = await client.GetAsync($"investing/stock/{symbol}?mod=mw_quote_tab", ct);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var htmlParser = _provider.GetRequiredService<IHtmlParser>();
            var htmlAsString = await response.Content.ReadAsStringAsync(ct);
            await htmlParser.Initialize(htmlAsString);

            var priceSectionNode = await htmlParser.FindFirstElement("//div[@class='intraday__data']/h2/bg-quote");
            if (priceSectionNode is null)
            {
                _logger.LogWarning("Node for given XPath not found");
                return default;
            }

            var conversionResult = priceSectionNode.Text.ToNullableDecimal();
            if (!conversionResult.HasValue)
            {
                _logger.LogWarning(
                    "Problem occured when parsing string {ValueToParse} to decimal number.",
                    priceSectionNode.Text);
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
