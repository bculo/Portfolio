using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility.Abstract
{
    /// <summary>
    /// Abstract class for typed clients
    /// </summary>
    public abstract class BaseHttpClient
    {
        private readonly ILogger? _logger;
        private IDictionary<string, string> _queryParameters;

        protected HttpClient Client { get; set; }
        

        public BaseHttpClient(HttpClient client)
        {
            Client = client;

            _queryParameters = new Dictionary<string, string>();
        }

        public BaseHttpClient(HttpClient client, ILogger logger) : this(client)
        {
            _logger = logger;
        }

        protected void AddQueryParameter(string key, string value)
        {
            var urlEncodedValue = WebUtility.UrlEncode(value);

            _logger?.LogTrace("Adding key value as query param {0} - {1}", key, urlEncodedValue);

            _queryParameters.Add(key, value);
        }

        protected void UpdateQueryParameter(string key, string value)
        {
            if (_queryParameters.TryGetValue(key, out _))
            {
                _logger?.LogTrace("Update process for query parameter with key {0} started", key);

                RemoveQueryParameter(key);
                AddQueryParameter(key, value);
            }
        }

        protected void RemoveQueryParameter(string key)
        {
            if(_queryParameters.TryGetValue(key, out _))
            {
                _logger?.LogTrace("Removing item with key {0}", key);

                _queryParameters.Remove(key);
            }
        }

        protected void ClearQueryParameters()
        {
            _logger?.LogTrace("Clearing query params");

            _queryParameters.Clear();
        }

        protected string BuildUrlWithQueryParameters(string url)
        {
            _logger?.LogTrace("Building url with query params. Base url: {0}", url);

            if (_queryParameters.Count == 0)
            {
                return url;
            }

            return QueryHelpers.AddQueryString(url, _queryParameters);
        }

        protected void AddHeader(string name, string value)
        {
            if (!Client.DefaultRequestHeaders.Contains(name))
            {
                _logger?.LogTrace("Adding header key value pair {0} - {1}", name, value);

                Client.DefaultRequestHeaders.Add(name, value);
            }
        }

        protected void RemoveHeader(string name)
        {
            if (!Client.DefaultRequestHeaders.Contains(name))
            {
                _logger?.LogTrace("Removing header item with key {0}", name);

                Client.DefaultRequestHeaders.Remove(name);
            }
        }

        protected void ClearHeader()
        {
            _logger?.LogTrace("Clearing header data");

            Client.DefaultRequestHeaders.Clear();        
        }
    }
}
