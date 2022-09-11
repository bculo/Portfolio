using HotChocolate.AspNetCore.Serialization;
using HotChocolate.Execution;
using System.Net;

namespace Crypto.GraphQL.Interceptors
{
    public class HttpResultSerializer : DefaultHttpResultSerializer
    {
        public override HttpStatusCode GetStatusCode(IExecutionResult result)
        {
            return base.GetStatusCode(result);
        }
    }
}
