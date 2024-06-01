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
        public static async Task<T> ExtractContentFromResponse<T>(this HttpResponseMessage? response, 
            ILogger? logger = null)
        {
            if (response is null)
            {
                return default!;
            }

            if (!response.IsSuccessStatusCode)
            {
                logger?.LogWarning("Request failed with status code {StatusCode}, Reason: {Reason}", 
                    response.StatusCode, 
                    response?.ReasonPhrase);
            }
            
            var stringResponse = await response!.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResponse)!;
        }

        public static async Task<string> ExtractContentFromResponse(this HttpResponseMessage response, ILogger? logger = null)
        {
            if (!response.IsSuccessStatusCode)
            {
                logger?.LogWarning("Request failed with status code {StatusCode}", response.StatusCode);
                return default!;
            }
            
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<T> HandleResponseWithException<T, TException>(this HttpResponseMessage response, ILogger? logger = null) where TException : Exception, new()
        {
            if (!response.IsSuccessStatusCode)
            {
                logger?.LogWarning("Request failed with status code {StatusCode}", response.StatusCode);
                throw new TException();
            }

            var stringResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResponse)!;
        }
    }
}
