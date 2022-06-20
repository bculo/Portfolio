using System.Reflection;
using Trend.Application;
using Trend.Grpc.Interceptors;
using Trend.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(opt =>
{
    opt.Interceptors.Add<TrendGrpExceptionInterceptor>();
    opt.EnableDetailedErrors = true;
});

builder.Services.AddGrpcReflection();

ApplicationLayer.AddServices(builder.Configuration, builder.Services);
//ApplicationLayer.AddBackgroundServies(builder.Configuration, builder.Services);
ApplicationLayer.AddLogger(builder);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<NewsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

IWebHostEnvironment env = app.Environment;
if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.Run();
