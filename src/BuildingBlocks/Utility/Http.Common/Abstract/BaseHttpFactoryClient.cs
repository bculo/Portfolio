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
    /// <summary>
    /// For HTTP client that use HttpClientFactory
    /// </summary>
    public abstract class BaseHttpFactoryClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private IDictionary<string, string> _queryParameters;
        private readonly ILogger? _logger;

        public BaseHttpFactoryClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;

            _queryParameters = new Dictionary<string, string>();
        }

        public BaseHttpFactoryClient(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;

            _queryParameters = new Dictionary<string, string>();
        }

        protected virtual HttpClient CreateNewClient(string clientName = null)
        {
            ClearQueryParameters();

            if(clientName == null)
            {
                return _clientFactory.CreateClient();
            }

            return _clientFactory.CreateClient(clientName);
        }

        protected void AddQueryParameter(string key, string value)
        {
            var urlEnvodedValue = WebUtility.UrlEncode(value);

            _queryParameters.Add(key, value);
        }

        protected void UpdateQueryParameter(string key, string value)
        {
            if (_queryParameters.TryGetValue(key, out _))
            {
                RemoveQueryParameter(key);
                AddQueryParameter(key, value);
            }
        }

        protected void RemoveQueryParameter(string key)
        {
            if (_queryParameters.TryGetValue(key, out _))
            {
                _queryParameters.Remove(key);
            }
        }

        protected void ClearQueryParameters()
        {
            _queryParameters.Clear();
        }

        protected string BuildUrlWithQueryParameters(string url)
        {
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
                client.DefaultRequestHeaders.Add(name, value);
            }
        }

        protected void RemoveHeader(HttpClient client, string name)
        {
            if (!client.DefaultRequestHeaders.Contains(name))
            {
                client.DefaultRequestHeaders.Remove(name);
            }
        }

        protected void ClearHeader(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
        }
    }
}
