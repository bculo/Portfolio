﻿using Crypto.Application.Interfaces.Services;
using Crypto.Application.Options;
using Crypto.Infrastracture.Clients;
using Crypto.Mock.Common.Data;
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

        private readonly IOptions<CryptoPriceApiOptions> _options;
        private readonly DefaultDataManager _dataManager = new DefaultDataManager();
        private static List<string> SUPPORTED_SYMBOLS = new List<string> { BTC_SYMBOL, ETH_SYMBOL, ADA_SYMBOL };
        private static List<string> INVALID_SYMBOLS = new List<string> { UNKNOWN_SYMBOL_V1, UNKNOWN_SYMBOL_V2 };

        public CryptoComapreClientTests()
        {
            var infoOptions = new CryptoPriceApiOptions
            {
                ApiKey = "Apikey ........",
                BaseUrl = "https://min-api.cryptocompare.com/data",
                HeaderKey = "authorization",
                Currency = USD_CURRENCY,
            };

            _options = Options.Create(infoOptions);

            _dataManager.InitSeedData(null);
            _dataManager.InitSupportedSymbols(SUPPORTED_SYMBOLS);
        }

        [Fact]
        public async Task GetPriceInfo_ShouldReturnPriceInformation_WhenCorrectSymbolProvided()
        {
            //Arrange
            string symbol = ETH_SYMBOL;
            var client = Build(true, false, symbol);

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
            var client = Build(true, false, symbol);

            //Act
            var response = await client.GetPriceInfo(symbol);

            //Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task GetPriceInfo_ShouldReturnPricesInformation_WhenCorrectSymbolsProvided()
        {
            //Arrange
            var symbols = _dataManager.GetSupportedCryptoSymbols();
            var client = Build(true, false, symbols.ToArray());

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
            var symbols = _dataManager.GetSupportedCryptoSymbols();
            symbols.Add(UNKNOWN_SYMBOL_V1);
            var client = Build(true, false, symbols.ToArray());

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
            var client = Build(true, false, symbols.ToArray());

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
            var client = Build(true, false, symbols.ToArray());

            //Act
            var result = await client.GetPriceInfo(symbols);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPriceInfo_ShouldThrowException_WhenTimeoutOccurs()
        {
            var symbols = new string[] { };
            var client = Build(true, true, symbols);

            await Assert.ThrowsAsync<TaskCanceledException>(() => client.GetPriceInfo(symbols.ToList()));
        }

        [Fact]
        public async Task GetPriceInfo_ShouldReturn_WhenClientUnauthorized()
        {
            var symbols = new string[] { };
            var client = Build(false, false, symbols);

            //Act
            var result = await client.GetPriceInfo(symbols.ToList());

            //Assert
            Assert.Null(result);
        }

        private ICryptoPriceService Build(bool isAuthorized, bool throwException, params string[] symbols)
        {
            var handler = new MockHttpMessageHandler();

            if(_dataManager.IsAnySymbolSupported(symbols) && isAuthorized && !throwException)
            {
                var validResponse = symbols.Length == 1 ? "{ 'USD': 20133.58 }" : MockResponse(symbols.ToList());
                handler.When("*").Respond("application/json", validResponse);
            }
            else if (!_dataManager.IsAnySymbolSupported(symbols) && isAuthorized && !throwException) //Invalid symbol
            {
                handler.When("*").Respond(HttpStatusCode.BadRequest, "applicaiton/json", BadResponseJson());
            }
            else if (!isAuthorized) //401
            {
                handler.When("*").Respond(HttpStatusCode.Unauthorized);
            }
            else if (throwException) //Task canceled exception (Timeout occurred)
            {
                handler.When("*").Throw(new TaskCanceledException());
            }
            else
            {
                handler.When("*").Respond(HttpStatusCode.BadRequest);
            }

            return new CryptoCompareClient(handler.ToHttpClient(), _options);
        }

        //{"BTC":{"USD":19703.13},"ETH":{"USD":1064.4},"ADA":{"USD":0.4303}} real server response
        private string MockResponse(List<string> symbols)
        {
            symbols.RemoveAll(i => INVALID_SYMBOLS.Contains(i));

            var response = new Dictionary<string, Dictionary<string, decimal>>();

            foreach (var symbol in symbols)
            {
                response.Add(symbol, new Dictionary<string, decimal> { { "USD", new Random().Next(1, 50000) } });
            }

            return JsonConvert.SerializeObject(response);
        }

        private string BadResponseJson()
        {
            return @"{
              'Response': 'Error',
              'Message': 'cccagg_or_exchange market does not exist for this coin pair (DRAGON123-EUR), cccagg_or_exchange market does not exist for this coin pair (DRAGON123-USD)',
              'HasWarning': false,
              'Type': 1,
              'RateLimit': {},
              'Data': {},
              'Cooldown': 0
            }";
        }
    }
}
