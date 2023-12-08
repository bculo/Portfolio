using MassTransit.Configuration;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using User.Application.Common.Options;

namespace User.Functions.Options;

public class ImplicitAuthFlow : OpenApiOAuthSecurityFlows
{
    public ImplicitAuthFlow()
    {
        var authorizationUrl = Environment.GetEnvironmentVariable("AuthorizationUrl") 
                               ?? throw new ArgumentNullException();

        Implicit = new OpenApiOAuthFlow()
        {
            AuthorizationUrl = new Uri(authorizationUrl),
        };
    }
}