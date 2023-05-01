namespace Tests.Common.Extensions;

public static class HttpClientExtensions
{
    public static HttpClient AddHeaderValue(this HttpClient client, string key, string value)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(value, nameof(value));
            
        if (client.DefaultRequestHeaders.Contains(key))
        {
            client.RemoveHeaderValue(key);
        }

        client.DefaultRequestHeaders.Add(key, value);
        return client;
    }

    public static HttpClient RemoveHeaderValue(this HttpClient client, string key)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
            
        if(client.DefaultRequestHeaders.Contains("Authorization"))
        {
            client.DefaultRequestHeaders.Remove("Authorization");
        }
            
        return client;
    }
}