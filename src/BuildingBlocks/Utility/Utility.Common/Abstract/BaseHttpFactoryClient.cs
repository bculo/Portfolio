using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Http.Common.Abstract
{
    public abstract class BaseHttpFactoryClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private IDictionary<string, string> _queryParameters;
        private readonly ILogger? _logger;

        protected BaseHttpFactoryClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;

            _queryParameters = new Dictionary<string, string>();
        }

        protected BaseHttpFactoryClient(IHttpClientFactory clientFactory, ILogger logger) : this(clientFactory)
        {
            _logger = logger;
        }

        protected virtual HttpClient CreateNewClient(string? clientName = null)
        {
            ClearQueryParameters();

            if (clientName == null)
            {
                _logger?.LogTrace("Creating HTTP client");

                return _clientFactory.CreateClient();
            }

            _logger?.LogTrace("Creating named {0} HTTP client", clientName);

            return _clientFactory.CreateClient(clientName);
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
            if (_queryParameters.TryGetValue(key, out _))
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

        protected void AddHeader(HttpClient client, string name, string value)
        {
            if (!client.DefaultRequestHeaders.Contains(name))
            {
                _logger?.LogTrace("Adding header key value pair {0} - {1}", name, value);

                client.DefaultRequestHeaders.Add(name, value);
            }
        }

        protected void RemoveHeader(HttpClient client, string name)
        {
            if (!client.DefaultRequestHeaders.Contains(name))
            {
                _logger?.LogTrace("Removing header item with key {0}", name);

                client.DefaultRequestHeaders.Remove(name);
            }
        }

        protected void ClearHeader(HttpClient client)
        {
            _logger?.LogTrace("Clearing header data");

            client.DefaultRequestHeaders.Clear();
        }
    }
}
