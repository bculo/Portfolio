using AutoFixture;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Price;
using Crypto.IntegrationTests.SeedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.MockImplementations
{
    public class MockCryptoPriceService : ICryptoPriceService
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly string[] AvailableSymbols = CryptoStaticData.GetSupportedCryptoSymbols();

        public Task<CryptoPriceResponseDto> GetPriceInfo(string symbol)
        {
            if (!AvailableSymbols.Contains(symbol.ToUpper()))
            {
                return null;
            }

            var response = _fixture.Create<CryptoPriceResponseDto>();

            response.Symbol = symbol;
            response.Symbol = "USD";

            return Task.FromResult(response);
        }

        public Task<List<CryptoPriceResponseDto>> GetPriceInfo(List<string> symbols)
        {
            symbols = symbols.Select(i => i.ToUpper()).ToList();

            if(!AvailableSymbols.Any(i => symbols.Contains(i)))
            {
                return null;
            }

            var symbolsToTrack = AvailableSymbols.Intersect(symbols).ToList();

            if (!symbolsToTrack.Any())
            {
                return null;
            }

            var response = symbolsToTrack.Select(i =>
            {
                var response = _fixture.Create<CryptoPriceResponseDto>();

                response.Symbol = i;
                response.Symbol = "USD";

                return response;
            }).ToList();

            return Task.FromResult(response);
        }
    }
}
