using FluentValidation.AspNetCore;
using HttpUtility.Extensions;
using MongoDB.Driver;
using Serilog;
using System.Diagnostics;
using Time.Common;
using Time.Common.Contracts;
using Trend.API.Filters;
using Trend.Application;
using Trend.Application.Background;
using Trend.Application.Clients;
using Trend.Application.Configurations.Persistence;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Repositories;
using Trend.Application.Options;
using Trend.Application.Repositories;
using Trend.Application.Services;
using Trend.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cl) =>
{
    cl.ReadFrom.Configuration(ctx.Configuration);

    cl.Enrich.FromLogContext();
    cl.Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName);
    cl.Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName);

    cl.WriteTo.MongoDBBson(cfg =>
    {
        var identity = new MongoInternalIdentity(ctx.Configuration["SerilogMongo:AuthDatabase"], ctx.Configuration["SerilogMongo:UserName"]);
        var evidence = new PasswordEvidence(ctx.Configuration["SerilogMongo:Password"]);
        
        var mongoDbSettings = new MongoClientSettings
        {
            UseTls = false,
            Credential = new MongoCredential(ctx.Configuration["SerilogMongo:AuthMechanisam"], identity, evidence),
            Server = new MongoServerAddress(ctx.Configuration["SerilogMongo:Host"], ctx.Configuration.GetValue<int>("SerilogMongo:Port")),
        };

        var mongoDbInstance = new MongoClient(mongoDbSettings).GetDatabase(ctx.Configuration["SerilogMongo:Database"]);

        cfg.SetMongoDatabase(mongoDbInstance);
    });
});

Serilog.Debugging.SelfLog.Enable(msg => {
    Debug.WriteLine(msg);
});

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
}).AddFluentValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<GoogleSearchOptions>(builder.Configuration.GetSection("GoogleSearchOptions"));
builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection("MongoOptions"));
builder.Services.Configure<SyncBackgroundServiceOptions>(builder.Configuration.GetSection("SyncBackgroundServiceOptions"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
builder.Services.AddScoped<ISyncStatusRepository, SyncStatusRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ISyncSettingRepository, SyncSettingRepository>();
//builder.Services.AddScoped<IMongoContext, MongoContext>();
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

MongoConfiguration.Configure();

builder.Services.AddScoped<IGoogleSyncService, GoogleSyncService>();
builder.Services.AddScoped<IGoogleSearchClient, GoogleSearchClient>();
builder.Services.AddScoped<IDateTime, LocalDateTimeService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ISyncService, SyncService>();


builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(typeof(ApplicationLayer).Assembly));

//builder.Services.AddHostedService<SyncBackgroundService>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
