using Crypto.Core.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Newtonsoft.Json;
using System.Text;

namespace Crypto.gRPC.Filters
{
    public class ExceptionInterceptor : Interceptor
    {
        private readonly ILogger<ExceptionInterceptor> _logger;

        public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, 
            ServerCallContext context, 
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await base.UnaryServerHandler(request, context, continuation);
            }
            catch (Exception exception)
            {
                throw HandleGrpcException(exception);
            }
        }

        private RpcException HandleGrpcException(Exception exception)
        {
            _logger.LogError(exception.Message, exception);

            var metadata = DefineMetadata(exception);

            if (exception is CryptoCoreException cryptoException)
            {
                return HandleCustomException(cryptoException!, metadata);
            }

            return new RpcException(new Status(StatusCode.Internal, "Unknown exception"), metadata);
        }

        private Metadata DefineMetadata(Exception exception)
        {
            string exceptionJson = JsonConvert.SerializeObject(exception,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

            byte[] exceptionByteArray = Encoding.UTF8.GetBytes(exceptionJson);

            return new Metadata
            {
                { "exception-bin", exceptionByteArray },
                { "exception-encoding", "utf8" },
            };
        }

        private RpcException HandleCustomException(CryptoCoreException grpcException, Metadata metadata)
        {
            if (grpcException is CryptoCoreNotFoundException)
            {
                return new RpcException(new Status(StatusCode.NotFound, grpcException.UserMessage), metadata);
            }

            return new RpcException(new Status(StatusCode.Internal, grpcException.UserMessage), metadata);
        }
    }
}
