using System.Diagnostics.SymbolStore;
using System.Threading.Channels;
using AutoFilterer.Abstractions;
using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Extensions;
using AutoFilterer.Types;
using Crypto.Application.Interfaces.Repositories;
using Crypto.Console.Playground.Mocks;
using Crypto.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Queryable.Common.Extensions;
using Queryable.Common.Models;
using Queryable.Common.Services.Dynamic;

var optionsBuilder = new DbContextOptionsBuilder<CryptoDbContext>();
optionsBuilder.UseLowerCaseNamingConvention();
optionsBuilder.EnableSensitiveDataLogging();
optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

var mockConnectionSting = Substitute.For<IConnectionProvider>();
mockConnectionSting.GetConnectionString()
    .Returns("Host=localhost;Port=5433;Database=Crypto;User Id=postgres;Password=florijan;");

var timeProvider = new MockTimeProvider();

using var dbContext = new CryptoDbContext(optionsBuilder.Options, mockConnectionSting, timeProvider);




// Approach 1
var builder = DynamicExpressionBuilder<Crypto.Core.Entities.VisitEntity>.Create();

var request = new
{
    SymbolString = "BTC",
    Symbol = new ContainFilter("BTC"),
    Name = new ContainFilter("Bitcoin"),
};




// Approach 2
var queryFilters = new List<QueryFilter>
{
    new QueryFilter
    {
        Operation = Operation.Contains,
        Value = "BTC",
        PropertyName = nameof(Crypto.Core.Entities.CryptoEntity.Symbol)
    },
    new QueryFilter
    {
        Operation = Operation.Contains,
        Value = "Bitcoin",
        PropertyName = nameof(Crypto.Core.Entities.CryptoEntity.Name)
    },
};

var result2 = await dbContext.Cryptos
    .Where(queryFilters)
    .ToListAsync();


// Approach 3
var autoFiltererRequest = new FilterDto
{
    Symbol = "BTC",
    Name = "Bitcoin"
};

var result3 = await dbContext.Cryptos
    .ApplyFilter(autoFiltererRequest)
    .ToListAsync();

Console.WriteLine("STOP");

public class FilterDto : FilterBase
{
    [CompareTo( nameof(Crypto.Core.Entities.CryptoEntity.Symbol))]
    [StringFilterOptions(StringFilterOption.Contains)]
    public string? Symbol { get; set; }

    [CompareTo(nameof(Crypto.Core.Entities.CryptoEntity.Name))]
    [StringFilterOptions(StringFilterOption.Contains)]
    public string? Name { get; set; }
}









