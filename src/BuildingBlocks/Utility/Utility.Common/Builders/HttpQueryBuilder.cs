using System.Net;
using Microsoft.AspNetCore.WebUtilities;

namespace Http.Common.Builders;

public class HttpQueryBuilder
{
    private readonly IDictionary<string, string?> _queryParameters = new Dictionary<string, string?>();
    
    public void Add(string key, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        
        var urlEncodedValue = WebUtility.UrlEncode(value);
        _queryParameters.Add(key, urlEncodedValue);
    }
    
    public void Remove(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        
        if (_queryParameters.TryGetValue(key, out _))
        {
            _queryParameters.Remove(key);
        }
    }

    public void Clear()
    {
        _queryParameters.Clear();
    }

    public string Build()
    {
        return QueryHelpers.AddQueryString("", _queryParameters);
    }
}