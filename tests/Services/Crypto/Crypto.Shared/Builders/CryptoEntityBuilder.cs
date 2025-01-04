using Bogus;
using Crypto.Core.Entities;
using Crypto.Shared.Utilities;
using Tests.Common.Interfaces.Builders;

namespace Crypto.Shared.Builders;

public class CryptoEntityBuilder : IObjectBuilder<CryptoEntity>
{
    private readonly CryptoEntity _entity = new Faker<CryptoEntity>()
        .RuleFor(x => x.Symbol, _ => SymbolGenerator.Generate())
        .RuleFor(x => x.Name, (_, u) => u.Symbol)
        .RuleFor(x => x.Description, f => f.Lorem.Sentence())
        .RuleFor(x => x.ModifiedOn, _ => DateTimeOffset.UtcNow)
        .RuleFor(x => x.CreatedOn, _ => DateTimeOffset.UtcNow)
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