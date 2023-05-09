
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

            return Task.FromResult(CryptoInfoModelMock.Mock(symbol));
        }
    }

    public static class CryptoInfoModelMock 
    {
        public static CryptoInfoResponseDto Mock(string symbol)
        {
            Fixture fixture = new();

            var rootResponse = fixture.Create<CryptoInfoResponseDto>();

            var data = fixture.Build<CryptoInfoDataDto>()
                .With(x => x.Symbol, symbol)
                .Create();

            data.Urls.Add("website", new string[] { " crypto-url" });
            data.Urls.Add("source_code", new string[] { "crypot-source_code" });

            rootResponse.Data.Clear();
            rootResponse.Data.Add(symbol, new List<CryptoInfoDataDto> { data });

            return rootResponse;
        }
    }
}
