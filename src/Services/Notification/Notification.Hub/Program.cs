using Notification.Hub;
using Keycloak.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Cryptography.Common.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureAuthentication(builder);

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PortfolioHub>("/portfolio");

app.Run();

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    builder.Services.UseKeycloakClaimServices(builder.Configuration["KeycloakOptions:ApplicationName"]);

    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = builder.Configuration.GetValue<bool>("AuthOptions:ValidateAudience"),
            ValidateIssuer = builder.Configuration.GetValue<bool>("AuthOptions:ValidateIssuer"),
            ValidIssuers = new[] { builder.Configuration["AuthOptions:ValidIssuer"] },
            ValidateIssuerSigningKey = builder.Configuration.GetValue<bool>("AuthOptions:ValidateIssuerSigningKey"),
            IssuerSigningKey = RsaUtils.ImportSubjectPublicKeyInfo(builder.Configuration["AuthOptions:PublicRsaKey"]),
            ValidateLifetime = builder.Configuration.GetValue<bool>("AuthOptions:ValidateLifetime")
        };

        opt.Events = new JwtBearerEvents()
        {
            OnTokenValidated = context =>
            {
                Console.WriteLine("User successfully authenticated");
                return Task.CompletedTask;
            },

            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/portfolio")))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Failed authentication");
                return Task.CompletedTask;
            }
        };
    });
}
