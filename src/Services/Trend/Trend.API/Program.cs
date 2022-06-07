using HttpUtility.Extensions;
using MongoDB.Driver;
using Serilog;
using System.Diagnostics;
using Trend.API.Filters;
using Trend.Application.Clients;
using Trend.Application.Interfaces;
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
    }, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);
});

Serilog.Debugging.SelfLog.Enable(msg => {
    Debug.WriteLine(msg);
});

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<GoogleSearchOptions>(builder.Configuration.GetSection("GoogleSearchOptions"));
builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection("MongoOptions"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
builder.Services.AddScoped<IArticleService, ArticleService>();

builder.Services.AddHttpClient<IGoogleSearchService, GoogleSearchClient>().ConfigureRetryPolicy();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
