using Tests.Common.Interfaces.Claims.Models;

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
            
        if(client.DefaultRequestHeaders.Contains(key))
        {
            client.DefaultRequestHeaders.Remove(key);
        }
            
        return client;
    }

    public static HttpClient WithRole(this HttpClient client, UserRole role)
    {
        client.RemoveHeaderValue("UserAuthType");
        
        return role switch
        {
            UserRole.None => client.RemoveHeaderValue("UserAuthType"),
            UserRole.User => client.AddHeaderValue("UserAuthType", ((int)UserRole.User).ToString()),
            UserRole.Admin => client.AddHeaderValue("UserAuthType", ((int)UserRole.Admin).ToString()),
            _ => client
        };
    }
    
    public static HttpClient AddJwtToken(this HttpClient client, string token)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        ArgumentNullException.ThrowIfNull(token, nameof(token));

        if(client.DefaultRequestHeaders.Contains("Authorization"))
        {
            client.RemoveJwtToken();
        }

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        return client;
    }

    public static HttpClient RemoveJwtToken(this HttpClient client)
    {
        ArgumentNullException.ThrowIfNull(client, nameof(client));
        
        if(client.DefaultRequestHeaders.Contains("Authorization"))
        {
            client.DefaultRequestHeaders.Remove("Authorization");
        }
        
        return client;
    }
}