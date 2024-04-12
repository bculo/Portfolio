using Crypto.Application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Crypto.Infrastructure.Persistence;

public class DbContextConnectionProvider : IConnectionProvider
{
    private readonly IConfiguration _config;

    private string? _connectionString = null;
    
    public DbContextConnectionProvider(IConfiguration config)
    {
        _config = config;
    }
    
    public string GetConnectionString()
    {
        return _connectionString ?? _config.GetConnectionString("CryptoDatabase");
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