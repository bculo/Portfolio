using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Keycloak.Common.Extensions
{
    internal static class KeycloakHttpResponseMessageExtensions
    {
        public static async Task<T> HandleResponse<T>(this HttpResponseMessage response, ILogger? logger = null)
        {
            logger?.LogTrace("Response received...");

            ArgumentNullException.ThrowIfNull(response);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();

                logger?.LogWarning(
                    "Keycloak request failed with status code {StatusCode}. Reason: {Reason}, Details {Error}",
                    response.StatusCode,
                    response.ReasonPhrase,
                    errorResponse);

                return default;
            }

            logger?.LogTrace("Parsing valid response...");

            var responseJson = await response.Content.ReadAsStringAsync();

            logger?.LogTrace("Parsing json response...");

            return JsonSerializer.Deserialize<T>(responseJson);
        }
    }
}
