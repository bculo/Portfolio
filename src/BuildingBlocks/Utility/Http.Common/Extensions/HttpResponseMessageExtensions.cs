using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.Common.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> HandleResponse<T>(this HttpResponseMessage response, ILogger logger = null)
        {
            if (response == null)
            {
                return default!;
            }

            if (!response.IsSuccessStatusCode)
            {
                logger?.LogWarning("Request failed with status code {0}", response.StatusCode);
                return default!;
            }

            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringResponse)!;
        }

        public static async Task<T> HandleResponseWithException<T, TException>(this HttpResponseMessage response, ILogger logger = null) where TException : Exception, new()
        {
            if (response == null)
            {
                throw new TException();
            }

            if (!response.IsSuccessStatusCode)
            {
                logger?.LogWarning("Request failed with status code {0}", response.StatusCode);
                throw new TException();
            }

            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringResponse)!;
        }
    }
}
