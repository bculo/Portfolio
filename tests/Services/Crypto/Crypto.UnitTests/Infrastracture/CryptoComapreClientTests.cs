using Crypto.Application.Interfaces.Services;
using Crypto.Application.Options;
using Crypto.Infrastracture.Clients;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.UnitTests.Infrastracture
{
    public class CryptoComapreClientTests
    {
        private const string BTC_SYMBOL = "BTC";
        private const string UNKNOWN_SYMBOL = "DRAGON123";

        private const string USD_CURRENCY = "USD";

        [Fact]
        public async Task GetPriceInfo_Should_Return_Price_Information_When_Correct_Symbol_Provided()
        {
            var client = Build(BTC_SYMBOL);

            var response = await client.GetPriceInfo(BTC_SYMBOL);

            Assert.NotNull(response);
            Assert.Equal(BTC_SYMBOL, response.Symbol);
            Assert.Equal(USD_CURRENCY, response.Currency);
            Assert.True(response.Price > 0.0m);
        }

        [Fact]
        public async Task GetPriceInfo_Should_Return_Null_When_Incorrect_Symbol_Provided()
        {
            var client = Build(UNKNOWN_SYMBOL);

            var response = await client.GetPriceInfo(UNKNOWN_SYMBOL);

            Assert.Null(response);
        }

        public ICryptoPriceService Build(string symbol)
        {
            var infoOptions = new CryptoPriceApiOptions
            {
                ApiKey = "Apikey ...........",
                BaseUrl = "https://min-api.cryptocompare.com/data",
                HeaderKey = "authorization",
                Currency = USD_CURRENCY,
            };

            var options = Options.Create(infoOptions);

            var handler = new MockHttpMessageHandler();

            if (symbol == BTC_SYMBOL)
            {
                string btcResponseJson = File.ReadAllText("./Static/btc-price-success-response.json");
                handler.When("https://min-api.cryptocompare.com/data/*")
                    .Respond("application/json", btcResponseJson);
            }
            else
            {
                string badResponseJson = File.ReadAllText("./Static/price-bad-response.json");
                handler.When("https://min-api.cryptocompare.com/data/*")
                    .Respond("application/json", badResponseJson);
            }

            return new CryptoCompareClient(handler.ToHttpClient(), options);
        }
    }
}
