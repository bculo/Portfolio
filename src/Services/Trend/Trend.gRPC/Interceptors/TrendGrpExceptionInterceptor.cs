using Grpc.Core;
using Grpc.Core.Interceptors;
using Newtonsoft.Json;
using System.Text;
using Trend.Domain.Exceptions;
using Trend.gRPC.Interceptors.Models;

namespace Trend.gRPC.Interceptors
{
    public class TrendGrpExceptionInterceptor(ILogger<TrendGrpExceptionInterceptor> logger) : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, 
            ServerCallContext context, 
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await base.UnaryServerHandler(request, context, continuation);
            }
            catch(Exception exception)
            {
                throw HandleGrpcException(exception);
            }
        }

        public override async Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, 
            IServerStreamWriter<TResponse> responseStream, 
            ServerCallContext context, 
            ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                await base.ServerStreamingServerHandler(request, responseStream, context, continuation);
            }
            catch (Exception exception)
            {
                throw HandleGrpcException(exception);
            }
        }

        public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream, 
            IServerStreamWriter<TResponse> responseStream, 
            ServerCallContext context, 
            DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                await base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
            }
            catch(Exception exception)
            {
                throw HandleGrpcException(exception);
            }
        }

        public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream, 
            ServerCallContext context, 
            ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await base.ClientStreamingServerHandler(requestStream, context, continuation);
            }
            catch(Exception exception)
            {
                throw HandleGrpcException(exception);
            }
        }

        private RpcException HandleGrpcException(Exception exception)
        {
            logger.LogError(exception.Message, exception);

            var metadata = DefineMetadata(exception);

            if (exception is TrendAppCoreException)
            {
                var customException = exception as TrendAppCoreException;
                return HandleCustomException(customException!, metadata);
            }

            return new RpcException(new Status(StatusCode.Internal, "Unknown exception"), metadata);
        }

        private Metadata DefineMetadata(Exception exception)
        {
            var exceptionJson = JsonConvert.SerializeObject(exception, 
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            var exceptionByteArray = Encoding.UTF8.GetBytes(exceptionJson);
            return new Metadata
            {
                { "exception-bin", exceptionByteArray },
                { "exception-encoding", "utf8" },
            };
        }

        private RpcException HandleCustomException(TrendAppCoreException grpcException, Metadata metadata)
        {
            if(grpcException is TrendValidationException)
            {
                var validationException = grpcException as TrendValidationException;

                var validationResponse = new ValidationResponse
                {
                    Errors = validationException!.Errors,
                    Message = validationException!.Message,
                    Title = validationException!.Title,
                };

                var responseJson = JsonConvert.SerializeObject(validationResponse, Formatting.Indented);

                return new RpcException(new Status(StatusCode.InvalidArgument, responseJson), metadata);
            }

            if(grpcException is TrendNotFoundException)
            {
                return new RpcException(new Status(StatusCode.NotFound, grpcException.UserMessage), metadata);
            }

            return new RpcException(new Status(StatusCode.Internal, grpcException.UserMessage), metadata);
        }
    }
}
