using System.Text;
using Newtonsoft.Json;
using System.Web;

namespace Tests.Common.Extensions;

public static class ObjectExtensions
{
    public static HttpContent AsHttpContent(this object request, string mediaType = "application/json")
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        string requestJson = System.Text.Json.JsonSerializer.Serialize(request);
        return new StringContent(requestJson, Encoding.UTF8, mediaType);
    }

    public static string AsQueryParametersString(this object request)
    {
        var serializedObject = JsonConvert.SerializeObject(request);
        var objectAsDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(serializedObject);
        var urlEncodedDictionary = objectAsDictionary
            .Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value));
        return string.Join("&", urlEncodedDictionary);
    }
}