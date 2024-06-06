using Crypto.Application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Crypto.Infrastructure.Persistence;

public class DbContextConnectionProvider(IConfiguration config) : IConnectionProvider
{
    private string? _connectionString = null;

    public string GetConnectionString()
    {
        return _connectionString ?? config.GetConnectionString("CryptoDatabase");
    }

    public void SetConnectionString(string connectionString, string dbName)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = dbName
        };
        
        _connectionString = builder.ToString();
    }
}