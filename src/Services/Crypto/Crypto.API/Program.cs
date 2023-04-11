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
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Crypto.API.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
});

builder.Host.UseSerilog((host, log) =>
{
    log.MinimumLevel.Debug();
    log.MinimumLevel.Override("Microsoft", LogEventLevel.Debug);
});

builder.Services.ConfigureVersioning(builder.Configuration);

builder.Services.AddOpenTelemetry()
    .WithTracing(builder =>
    {
        builder
            .AddSource("MassTransit")
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("Crypto.API")
                .AddTelemetrySdk()
                .AddEnvironmentVariableDetector())
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation(opt =>
            {
                opt.RecordException = true;
                opt.EnableConnectionLevelAttributes = true;
                opt.SetDbStatementForText = true;

                opt.Filter = cmd =>
                {
                    if (cmd is SqlCommand command 
                        && (command.CommandText.Contains("OutboxState")
                            || command.CommandText.Contains("InboxState")))
                    {
                        return false;
                    }
                    return true;
                };
            })
            .AddJaegerExporter(o =>
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
    app.UseSwaggerUI(options =>
    {
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();

//For testing purpose
public partial class Program { }
