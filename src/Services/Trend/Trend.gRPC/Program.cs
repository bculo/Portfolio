using Trend.gRPC.Extensions;
using Trend.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.ConfiguregRPCProject();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<NewsService>();
app.MapGrpcService<GreeterService>();

IWebHostEnvironment env = app.Environment;

if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();

//For testing purpose
public partial class Program { }