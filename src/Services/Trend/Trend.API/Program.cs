using HttpUtility.Extensions;
using Trend.API.Filters;
using Trend.API.Infrastructure.Clients;
using Trend.API.Infrastructure.Repositories;
using Trend.API.Interfaces;
using Trend.API.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<GoogleSearchOptions>(builder.Configuration.GetSection("GoogleSearchOptions"));
builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection("MongoOptions"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

builder.Services.AddHttpClient<IGoogleSearchService, GoogleSearchClient>().ConfigureRetryPolicy();

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
