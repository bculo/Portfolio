using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Price.Models;
using Crypto.Application.Interfaces.Repositories;
using Time.Abstract.Contracts;

namespace Crypto.Infrastructure.Price;

public class MockPriceClient : ICryptoPriceService
{
    private readonly IUnitOfWork _work;
    private readonly IDateTimeProvider _provider;

    public MockPriceClient(IDateTimeProvider provider, IUnitOfWork work)
    {
        _provider = provider;
        _work = work;
    }

    public Task<PriceResponse> GetPriceInfo(string symbol, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<PriceResponse>> GetPriceInfo(List<string> symbols, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}