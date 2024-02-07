using Tests.Common.Interfaces.Text;

namespace Stock.API.IntegrationTests.Data;

public class MarketDataLoader : ITextLoader
{
    public async Task<string> LoadAsync()
    {
        var path = Path.Combine("Data", "marketdata.html");
        
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("marketdata.html");
        }

        return await File.ReadAllTextAsync(path);
    }
}