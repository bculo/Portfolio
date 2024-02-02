using Microsoft.EntityFrameworkCore;
using Queryable.Common.Extensions;
using Queryable.Common.Models;
using Queryable.Common.Services.Dynamic;
using Stock.Console.Playground.Mocks;
using Stock.Core.Models.Stock;

/*
var filters = new List<QueryFilter>
{
    new QueryFilter
    {
        Operation = Operation.GreaterThan,
        Value = 10000m,
        PropertyName = "Price"
    }
};
*/

var filterSymbol = new ContainFilter("A");
var filterPrice = new GreaterThanFilter<decimal>(15000m);
var builder = DynamicExpressionBuilder<StockWithPriceTag>.Create();

builder.Add(i => i.Symbol, filterSymbol)
    .Add(i => i.Price, filterPrice);

await using var context = DbContextMock.CreateContext();
var set = context.StockWithPriceTag.AsSingleQuery();

foreach (var filter in builder.Build())
{
    set = set.Where(filter);
}

var result = await set.ToListAsync();

Console.WriteLine("END");



