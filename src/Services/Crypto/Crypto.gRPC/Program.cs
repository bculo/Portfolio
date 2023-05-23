using Crypto.gRPC.Configurations;
using Crypto.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfiguregRPCApplication(builder.Configuration);

var app = builder.Build();

app.UseGrpcWeb();
app.MapGrpcService<CryptoService>().EnableGrpcWeb();
app.MapGrpcService<GreeterService>().EnableGrpcWeb();
app.MapGrpcReflectionService();

app.Run();
