using AutoFixture;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.IntegrationTests.SeedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.MockImplementations
{
    public class MockCryptoInfoService : ICryptoInfoService
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly string[] AvailableSymbols = CryptoStaticData.GetSupportedCryptoSymbols();

        public MockCryptoInfoService(HttpClient client)
        {

        }

        public Task<CryptoInfoResponseDto> FetchData(string symbol)
        {
            if (!AvailableSymbols.Contains(symbol.ToUpper()))
            {
                return null;
            }

            var response = _fixture.Create<CryptoInfoResponseDto>();

            var value = response.Data.First().Value.First();

            value.Symbol = symbol;
            value.Name = symbol;
            value.Description = symbol;

            response.Data.Clear();
            response.Data.Add(symbol, new List<CryptoInfoDataDto> { value });

            return Task.FromResult(response);
        }
    }
}
