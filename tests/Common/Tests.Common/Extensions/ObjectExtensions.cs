using System.Text;

namespace Tests.Common.Extensions;

public static class ObjectExtensions
{
    public static HttpContent AsHttpContent(this object request, string mediaType = "application/json")
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        string requestJson = System.Text.Json.JsonSerializer.Serialize(request);
        return new StringContent(requestJson, Encoding.UTF8, mediaType);
    }
}