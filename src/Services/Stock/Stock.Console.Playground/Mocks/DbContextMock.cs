using Microsoft.Extensions.Configuration;
using NSubstitute;
using Stock.Application.Interfaces.User;
using Stock.Infrastructure.Persistence;
using Time.Abstract.Contracts;

namespace Stock.Console.Playground.Mocks;

public static class DbContextMock
{
    public static StockDbContext CreateContext()
    {
        var inMemorySettings = new Dictionary<string, string> 
        {
            { "ConnectionStrings:StockDatabase", "Host=localhost;Port=5433;Database=Stock;User Id=postgres;Password=florijan;" },
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        IDateTimeProvider timeProvider = Substitute.For<IDateTimeProvider>();
        IStockUser userService = Substitute.For<IStockUser>();

        return new StockDbContext(configuration, timeProvider, userService);
    }
}