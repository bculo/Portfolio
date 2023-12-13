using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.OpenApi.Models;

namespace User.Functions.Options;

public class ImplicitAuthFlow : OpenApiOAuthSecurityFlows
{
    public ImplicitAuthFlow()
    {
        var authorizationUrl = Environment.GetEnvironmentVariable("AuthorizationUrl") 
                               ?? throw new ArgumentNullException();

        var refreshUrl = Environment.GetEnvironmentVariable("RefreshUrl") 
                               ?? throw new ArgumentNullException();

        Implicit = new OpenApiOAuthFlow()
        {
            AuthorizationUrl = new Uri(authorizationUrl),
            RefreshUrl = new Uri(refreshUrl),
        };
    }
}