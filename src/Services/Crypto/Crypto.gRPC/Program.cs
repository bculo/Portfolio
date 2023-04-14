using Crypto.gRPC.Configurations;
using Crypto.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfiguregRPCApplication(builder.Configuration);

var app = builder.Build();

app.MapGrpcService<CryptoService>();
app.MapGrpcService<GreeterService>();
app.MapGrpcReflectionService();

app.Run();
