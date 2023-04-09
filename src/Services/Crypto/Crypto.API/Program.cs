using Crypto.API.Filters;
using Crypto.Application;
using Crypto.Application.Interfaces.Services;
using Crypto.Infrastracture;
using Serilog;
using Serilog.Events;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
});

builder.Host.UseSerilog((host, log) =>
{
    log.MinimumLevel.Debug();
    log.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
    log.WriteTo.Console();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .WithTracing(config =>
    {
        config.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("Crypto.API")
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector());
        config.AddAspNetCoreInstrumentation();
        config.AddJaegerExporter(o =>
         {
             o.AgentHost = "localhost";
             o.AgentPort = 6831;
             o.MaxPayloadSizeInBytes = 4096;
             o.ExportProcessorType = ExportProcessorType.Batch;
             o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
             {
                 MaxQueueSize = 2048,
                 ScheduledDelayMilliseconds = 5000,
                 ExporterTimeoutMilliseconds = 30000,
                 MaxExportBatchSize = 512,
             };
         });
    });

ApplicationLayer.AddServices(builder.Services, builder.Configuration);

InfrastractureLayer.ConfigureMessageQueue(builder.Services, builder.Configuration);
InfrastractureLayer.AddServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

//For testing purpose
public partial class Program { }
