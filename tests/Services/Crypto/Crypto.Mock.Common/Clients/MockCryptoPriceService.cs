using AutoFixture;
using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Price;
using Crypto.Mock.Common.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Mock.Common.Clients
{
    public class MockCryptoPriceService : HttpBaseMockClient, ICryptoPriceService
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly string[] _availableSymbols;

        public MockCryptoPriceService(string[] availableSymbols, bool isAuthorized = true, bool throwTimeOutException = false)
            : base(isAuthorized, throwTimeOutException)
        {
            _availableSymbols = availableSymbols;
        }

        public Task<CryptoPriceResponseDto> GetPriceInfo(string symbol)
        {
            if (_throwTimeOutException)
            {
                throw new TaskCanceledException("Timeout exception occurred");
            }

            if (!_isAuthorized)
            {
                return Task.FromResult<CryptoPriceResponseDto>(null!);
            }

            if (!_availableSymbols.Contains(symbol.ToUpper()))
            {
                return Task.FromResult<CryptoPriceResponseDto>(null!);
            }

            var response = _fixture.Create<CryptoPriceResponseDto>();

            response.Symbol = symbol;
            response.Currency = "USD";

            return Task.FromResult(response);
        }

        public Task<List<CryptoPriceResponseDto>> GetPriceInfo(List<string> symbols)
        {
            if (_throwTimeOutException)
            {
                throw new TaskCanceledException("Timeout exception occurred");
            }

            if (!_isAuthorized)
            {
                return Task.FromResult<List<CryptoPriceResponseDto>>(null!);
            }

            symbols = symbols.Select(i => i.ToUpper()).ToList();

            if (!_availableSymbols.Any(i => symbols.Contains(i)))
            {
                return Task.FromResult<List<CryptoPriceResponseDto>>(null!);
            }

            var symbolsToTrack = _availableSymbols.Intersect(symbols).ToList();

            if (!symbolsToTrack.Any())
            {
                return Task.FromResult<List<CryptoPriceResponseDto>>(null!);
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
