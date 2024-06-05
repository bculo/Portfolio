using Grpc.Core;
using Grpc.Core.Interceptors;
using Stock.Core.Exceptions;
using Stock.gRPC.Handlers.Models;


namespace Stock.gRPC.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await base.UnaryServerHandler(request, context, continuation);
        }
        catch (Exception exception)
        {
            logger.LogError("Exception occured: {Exception}", exception);
            
            if (exception is StockCoreException coreException)
            {
                throw HandleCustomException(coreException);
            }

            throw new RpcException(new Status(StatusCode.Internal, "Unknown exception"));
        }
    }
    
    private RpcException HandleCustomException(StockCoreException grpcException)
    {
        if(grpcException is StockCoreValidationException validationException)
        {
            var validationResponse = new ValidationResponse
            {
                Errors = validationException!.Errors,
            };

            var responseJson = System.Text.Json.JsonSerializer.Serialize(validationResponse);

            return new RpcException(new Status(StatusCode.InvalidArgument, responseJson));
        }

        if(grpcException is StockCoreNotFoundException notFoundException)
        {
            return new RpcException(new Status(StatusCode.NotFound, notFoundException.Message));
        }

        return new RpcException(new Status(StatusCode.Internal, grpcException.Message));
    }
}