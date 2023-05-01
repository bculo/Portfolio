using System.Text;
using Newtonsoft.Json;

namespace Trend.IntegrationTests;

public static class HttpClientUtilities
{
    public static HttpContent PrepareJsonRequest(object request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        string requestJson = JsonConvert.SerializeObject(request);
        return new StringContent(requestJson, Encoding.UTF8, "application/json");
    }
}