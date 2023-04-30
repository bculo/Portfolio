using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests.Helpers
{
    public static class HttpClientExtensions
    {
        public static void AddJwtToken(this HttpClient client, string token)
        {
            ArgumentNullException.ThrowIfNull(client, nameof(client));
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            if (client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.RemoveJwtToken();
            }

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public static void RemoveJwtToken(this HttpClient client)
        {
            ArgumentNullException.ThrowIfNull(client, nameof(client));

            client.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
