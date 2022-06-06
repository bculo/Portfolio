using HttpUtility.Extensions;
using Trend.API.Infrastructure.Clients;
using Trend.API.Interfaces;
using Trend.API.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<GoogleSearchOptions>(builder.Configuration.GetSection("GoogleSearchOptions"));

builder.Services.AddHttpClient<IGoogleSearchService, GoogleSearchClient>()
    .AddPortfolioRetryPolicy();

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
