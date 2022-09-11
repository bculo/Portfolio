using HotChocolate.AspNetCore;
using HotChocolate.Execution;

namespace Crypto.GraphQL.Interceptors
{
    public class HttpRequestInterceptor : DefaultHttpRequestInterceptor
    {
        private readonly ILogger<HttpRequestInterceptor> _logger;

        public HttpRequestInterceptor(ILogger<HttpRequestInterceptor> logger)
        {
            _logger = logger;
        }

        public override ValueTask OnCreateAsync(HttpContext context, IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder, CancellationToken cancellationToken)
        {
            return base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }
    }
}
