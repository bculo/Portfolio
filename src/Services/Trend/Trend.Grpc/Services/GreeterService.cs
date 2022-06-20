using Grpc.Core;
using Trend.Grpc;

namespace Trend.Grpc.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<EmptyResponse> ThrowError(EmptyRequest request, ServerCallContext context)
        {
            throw new Exception("HELLLO");
        }
    }
}