using System.Text;
using Newtonsoft.Json;

namespace Tests.Common.Utilities;

public static class HttpClientUtilities
{
    public static HttpContent PrepareJsonRequest(object request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        string requestJson = JsonConvert.SerializeObject(request);
        return new StringContent(requestJson, Encoding.UTF8, "application/json");
    }
}