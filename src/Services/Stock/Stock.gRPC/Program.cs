using Stock.gRPC.Extensions;
using Stock.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureGrpcProject();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<StockService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();