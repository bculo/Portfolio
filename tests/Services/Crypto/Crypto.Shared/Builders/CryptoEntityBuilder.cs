using Bogus;
using Crypto.Core.Entities;
using Crypto.Shared.Utilities;
using Tests.Common.Interfaces.Builders;

namespace Crypto.Shared.Builders;

public class CryptoEntityBuilder : IObjectBuilder<CryptoEntity>
{
    private readonly CryptoEntity _entity = new Faker<CryptoEntity>()
        .RuleFor(x => x.Symbol, _ => SymbolGenerator.Generate())
        .Generate();

    public CryptoEntity Build() => _entity;

    public CryptoEntityBuilder WithName(string name)
    {
        _entity.Name = name;
        return this;
    }
    
    public CryptoEntityBuilder WithSymbol(string symbol)
    {
        _entity.Symbol = symbol;
        return this;
    }
}