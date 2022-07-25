using Crypto.Application.Interfaces.Services;
using Crypto.Application.Options;
using Crypto.Infrastracture.Clients;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.UnitTests.Infrastracture
{
    public class CryptoComapreClientTests
    {
        private const string BTC_SYMBOL = "BTC";
        private const string ETH_SYMBOL = "ETH";
        private const string ADA_SYMBOL = "ADA";

        private const string UNKNOWN_SYMBOL_V1 = "DRAGON123";
        private const string UNKNOWN_SYMBOL_V2 = "DRAGON1234";

        private const string USD_CURRENCY = "USD";

        private static string[] VALID_SYMBOLS = new string[] { BTC_SYMBOL, ETH_SYMBOL, ADA_SYMBOL };
        private static string[] INVALID_SYMBOLS = new string[] { UNKNOWN_SYMBOL_V1, UNKNOWN_SYMBOL_V2 };

        [Fact]
        public async Task GetPriceInfo_ShouldReturnPriceInformation_WhenCorrectSymbolProvided()
        {
            //Arrange
            string symbol = ETH_SYMBOL;
            var client = BuildClientForSignleSymbol(symbol);

            //Act
            var response = await client.GetPriceInfo(symbol);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(symbol, response.Symbol);
            Assert.Equal(USD_CURRENCY, response.Currency);
            Assert.True(response.Price >= 0.0m);
        }

        [Fact]
        public async Task GetPriceInfo_ShouldReturnNull_WhenIncorrectSymbolProvided()
        {
            //Arrange
            string symbol = UNKNOWN_SYMBOL_V1;
            var client = BuildClientForSignleSymbol(symbol);

            //Act
            var response = await client.GetPriceInfo(symbol);

            //Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task GetPriceInfo_ShouldReturnPricesInformation_WhenCorrectSymbolsProvided()
        {
            //Arrange
            var symbols = new List<string>(VALID_SYMBOLS);
            var client = BuildClientForMultipleSymbols(symbols);

            //Act
            var result = await client.GetPriceInfo(symbols);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Count, symbols.Count);
            Assert.All(result, i => symbols.Contains(i.Symbol));
        }

        [Fact]
        public async Task GetPriceInfo_ShouldReturnPrices_WhenSomeCorrectSymbolsProvided()
        {
            //Arrange
            var symbols = new List<string>(VALID_SYMBOLS);
            symbols.Add(UNKNOWN_SYMBOL_V1);
            var client = BuildClientForMultipleSymbols(symbols);

            //Act
            var result = await client.GetPriceInfo(symbols);

            //Assert
            Assert.NotNull(result);
            Assert.DoesNotContain(result, i => i.Symbol == UNKNOWN_SYMBOL_V1);
        }

        [Fact]
        public async Task GetPriceInfo_ShouldReturnNULL_WhenEmptyListPassedToMethod()
        {
            //Arrange
            var symbols = new List<string>();
            var client = BuildClientForMultipleSymbols(symbols);

            //Act
            var result = await client.GetPriceInfo(symbols);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPriceInfo_ShouldReturnNull_WhenAllIncorrectSymbolsProvided()
        {
            //Arrange
            var symbols = new List<string>(INVALID_SYMBOLS);
            var client = BuildClientForMultipleSymbols(symbols);

            //Act
            var result = await client.GetPriceInfo(symbols);

            //Assert
            Assert.Null(result);
        }


        private ICryptoPriceService BuildClientForMultipleSymbols(List<string> symbols)
        {
            var infoOptions = new CryptoPriceApiOptions
            {
                ApiKey = "Apikey ........",
                BaseUrl = "https://min-api.cryptocompare.com/data",
                HeaderKey = "authorization",
                Currency = USD_CURRENCY,
            };

            var options = Options.Create(infoOptions);

            var handler = new MockHttpMessageHandler();

            if(symbols.Any(i => VALID_SYMBOLS.Contains(i)))
            {
                var response = MockResponse(symbols);

                handler.When("https://min-api.cryptocompare.com/data/*")
                    .Respond("application/json", response);

                return new CryptoCompareClient(handler.ToHttpClient(), options);
            }

            handler.When("https://min-api.cryptocompare.com/data/*")
                .Respond(HttpStatusCode.BadRequest);

            return new CryptoCompareClient(handler.ToHttpClient(), options);
        }

        //{"BTC":{"USD":19703.13},"ETH":{"USD":1064.4},"ADA":{"USD":0.4303}} real server response
        private string MockResponse(List<string> symbols)
        {
            symbols.RemoveAll(i => INVALID_SYMBOLS.Contains(i));

            var response = new Dictionary<string, Dictionary<string, decimal>>();

            foreach(var symbol in symbols)
            {
                response.Add(symbol, new Dictionary<string, decimal> { { "USD", new Random().Next(1, 50000) } });
            }

            return JsonConvert.SerializeObject(response);
        }

        public ICryptoPriceService BuildClientForSignleSymbol(string symbol)
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

            if (VALID_SYMBOLS.Contains(symbol))
            {
                string validResponse = "{ 'USD': 20133.58 }";
                handler.When("https://min-api.cryptocompare.com/data/*")
                    .Respond("application/json", validResponse);
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
