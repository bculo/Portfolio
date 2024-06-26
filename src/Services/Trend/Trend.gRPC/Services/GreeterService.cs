using Grpc.Core;
using Trend.gRPC;
using Trend.gRPC.Protos.v1;

namespace Trend.gRPC.Services
{
    public class GreeterService(ILogger<GreeterService> logger) : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger = logger;

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}