using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.Utils
{
    public static class HttpClientUtilities
    {
        public static HttpContent PrepareJsonRequest(object request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            string requestJson = JsonConvert.SerializeObject(request);
            return new StringContent(requestJson, Encoding.UTF8, "application/json");
        }
    }
}
