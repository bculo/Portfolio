using Microsoft.Extensions.Configuration;

namespace Tests.Common.Extensions;

public static class DictionaryExtensions
{
    public static IConfigurationRoot AsConfiguration(this Dictionary<string, string> dict)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(dict!)
            .Build();
    } 
}