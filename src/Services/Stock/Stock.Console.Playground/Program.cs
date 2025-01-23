using Microsoft.EntityFrameworkCore;
using Queryable.Common.Models;
using Queryable.Common.Services.Dynamic;
using Stock.Console.Playground.Mocks;
using Stock.Core.Models.Stock;

var filterSymbol = new ContainFilter("A");
var filterPrice = new GreaterThanFilter<decimal>(15000m);
var builder = DynamicExpressionBuilder<StockWithPriceTag>.Create();

await using var context = DbContextMock.CreateContext();
var set = context.Set<StockWithPriceTag>().AsSingleQuery();

foreach (var filter in builder.Build())
{
    set = set.Where(filter);
}

var result = await set.ToListAsync();

Console.WriteLine("END");



