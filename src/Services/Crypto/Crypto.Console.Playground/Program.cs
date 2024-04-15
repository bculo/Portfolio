using Crypto.Console.Playground.Mocks;
using Crypto.Core.Entities;
using Crypto.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Time.Abstract.Contracts;

var optionsBuilder = new DbContextOptionsBuilder<CryptoDbContext>();
optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=Crypto;User Id=postgres;Password=florijan;");
optionsBuilder.UseLowerCaseNamingConvention();
optionsBuilder.EnableSensitiveDataLogging();
optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

var timeProvider = new MockTimeProvider();


