
using AutoFixture;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Mock.Common.Common;

namespace Crypto.Mock.Common.Clients
{
    public class MockCryptoInfoService : HttpBaseMockClient, ICryptoInfoService
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly string[] _availableSymbols;

        public MockCryptoInfoService(string[] availableSymbols, bool isAuthorized = true, bool throwTimeOutException = false)
            : base(isAuthorized, throwTimeOutException)
        {
            _availableSymbols = availableSymbols;
        }

        public Task<CryptoInfoResponseDto> FetchData(string symbol)
        {
            if (_throwTimeOutException)
            {
                throw new TaskCanceledException("Timeout exception occurred");
            }

            if (!_isAuthorized)
            {
                return Task.FromResult<CryptoInfoResponseDto>(null!);
            }

            if (!_availableSymbols.Contains(symbol.ToUpper()))
            {
                return Task.FromResult<CryptoInfoResponseDto>(null!);
            }

            var response = _fixture.Create<CryptoInfoResponseDto>();

            var value = response.Data.First().Value.First();

            value.Symbol = symbol;
            value.Name = symbol;
            value.Description = symbol;

            value.Urls.Add("website", new string[] { " crypto-url " });
            value.Urls.Add("source_code", new string[] { "crypot-source_code " });

            response.Data.Clear();
            response.Data.Add(symbol, new List<CryptoInfoDataDto> { value });

            return Task.FromResult(response);
        }
    }
}
