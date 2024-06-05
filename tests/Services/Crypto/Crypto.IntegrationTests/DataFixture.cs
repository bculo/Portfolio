using AutoFixture;
using Crypto.Application.Interfaces.Repositories;
using Tests.Common.Extensions;

namespace Crypto.IntegrationTests;

public class DataFixture
{
    private readonly IUnitOfWork _work;
    private readonly Fixture _fixture = new Fixture().Configure();

    public DataFixture(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task<Crypto.Core.Entities.Crypto> AddInstance(string name)
    {
        var crypto = _fixture.Create<Crypto.Core.Entities.Crypto>();
        crypto.Name = name;
        crypto.Symbol = name;

        await _work.CryptoRepo.Add(crypto);
        await _work.Commit();
        
        return crypto;
    }
}