using Trend.Grpc.Extensions;
using Trend.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.ConfiguregRPCProject();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapGrpcService<NewsService>();

IWebHostEnvironment env = app.Environment;

if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();
