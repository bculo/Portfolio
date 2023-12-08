using Azure.Core.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace User.Functions.Utilities;

public class SerializerUtilities
{
    public static ObjectSerializer Create()
    {
        var s = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        return new NewtonsoftJsonObjectSerializer(s);
    }
}