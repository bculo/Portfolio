using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Trend.Grpc.Interceptors
{
    public class TrendGrpExceptionInterceptor : Interceptor
    {
        private readonly ILogger<TrendGrpExceptionInterceptor> _logger;

        public TrendGrpExceptionInterceptor(ILogger<TrendGrpExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);


                throw new RpcException(new Status(StatusCode.Aborted, "Aborted"));
            }
        }
    }
}
