namespace Crypto.Application.Interfaces.Repositories;

public interface IConnectionProvider
{
    public string GetConnectionString();
    public void SetConnectionString(string connectionString, string dbName);
}