using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Extensions
{
    internal static class KeycloakHttpResponseMessageExtensions
    {
        public static async Task<T> HandleResponse<T>(this HttpResponseMessage response, ILogger logger = null)
        {
            logger?.LogTrace("Response received...");

            if(response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                logger?.LogWarning("Keycloak request failed with status code {0}. Reason: {1}, Details {2}",
                    response.StatusCode,
                    response.ReasonPhrase,
                    errorResponse);

                return default;
            }

            logger?.LogTrace("Parsing valid response...");

            var responseJson = await response.Content.ReadAsStringAsync();

            logger?.LogTrace("Parsing json response...");

            return JsonConvert.DeserializeObject<T>(responseJson);
        }
    }
}
