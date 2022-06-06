using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility.Abstract
{
    public abstract class BaseHttpClient
    {
        protected HttpClient Client { get; set; }

        private IDictionary<string, string> _queryParameters;

        public BaseHttpClient(HttpClient client)
        {
            Client = client;

            _queryParameters = new Dictionary<string, string>();
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
            if(_queryParameters.TryGetValue(key, out _))
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

        protected void AddHeader(string name, string value)
        {
            if (!Client.DefaultRequestHeaders.Contains(name))
            {
                Client.DefaultRequestHeaders.Add(name, value);
            }
        }

        protected void RemoveHeader(string name)
        {
            if (!Client.DefaultRequestHeaders.Contains(name))
            {
                Client.DefaultRequestHeaders.Remove(name);
            }
        }

        protected void ClearHeader()
        {
            Client.DefaultRequestHeaders.Clear();        
        }
    }
}
