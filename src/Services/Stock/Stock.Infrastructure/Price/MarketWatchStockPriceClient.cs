using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PrimitiveTypes.Common.Decimal;
using Stock.Application.Common.Constants;
using Stock.Application.Interfaces.Html;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Price.Models;
using Time.Abstract.Contracts;

namespace Stock.Infrastructure.Price
{
    public class MarketWatchStockPriceClient(
        IServiceProvider provider,
        ILogger<MarketWatchStockPriceClient> logger,
        IHttpClientFactory factory,
        IDateTimeProvider timeProvider)
        : IStockPriceClient
    {
        public async Task<StockPriceInfo?> GetPrice(string symbol, CancellationToken ct = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(symbol);

            var client = factory.CreateClient(HttpClientNames.MarketWatch);
            var response = await client.GetAsync($"investing/stock/{symbol}?mod=mw_quote_tab", ct);
            if (!response.IsSuccessStatusCode)
                return null;
            
            var htmlParser = provider.GetRequiredService<IHtmlParser>();
            var htmlAsString = await response.Content.ReadAsStringAsync(ct);
            await htmlParser.Initialize(htmlAsString);

            var priceSectionNode = await htmlParser.FindFirstElement("//div[@class='intraday__data']/h2/bg-quote");
            if (priceSectionNode is null)
            {
                logger.LogWarning("Node for given XPath not found");
                return null;
            }

            var conversionResult = priceSectionNode.Text.ToNullableDecimal(new CultureInfo("en-US"));
            if (!conversionResult.HasValue)
            {
                logger.LogWarning(
                    "Problem occured when parsing string {ValueToParse} to decimal number.",
                    priceSectionNode.Text);
                return null;
            }

            return new StockPriceInfo
            {
                Price = conversionResult.Value,
                Symbol = symbol,
                FetchedTimestamp = timeProvider.Now
            };
        }
    }
}
