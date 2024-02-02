using Microsoft.EntityFrameworkCore;
using Queryable.Common.Services;
using Stock.Console.Playground.Mocks;
using Stock.Core.Models.Stock;

var builder = ComplexExpressionBuilder<StockEntity>.Create();
builder.Add(m => m.CreatedAt, new LessThenFilter<DateTime>(DateTime.UtcNow.AddDays(-30)));
await using var context = DbContextMock.CreateContext();
var result = await context.Stocks.Where(builder.ToArray()[0]).ToListAsync();


Console.WriteLine("END");



