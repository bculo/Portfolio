using Crypto.Core.Entities;
using Crypto.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Time.Abstract.Contracts;

var optionsBuilder = new DbContextOptionsBuilder<CryptoDbContext>();
optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=Crypto;User Id=postgres;Password=florijan;");
optionsBuilder.UseLowerCaseNamingConvention();

var timeProvider = new TimeProvider();

await using var dbContext = new CryptoDbContext(optionsBuilder.Options, timeProvider);


var result = await dbContext.Prices.ToListAsync();


var priceId = 51239.32m;
var time = DateTimeOffset.UtcNow;
var cryptoId = Guid.Parse("7cff6f5b-de85-4a31-915d-4fa762697831");

await dbContext.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO public.crypto_price(\"time\", price, cryptoid) VALUES ({time}, {priceId}, {cryptoId});");

Console.WriteLine("DONE");





public class TimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
    public DateTime Utc => DateTime.UtcNow;
} 