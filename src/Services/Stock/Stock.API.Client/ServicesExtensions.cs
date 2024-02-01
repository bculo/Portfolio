using Api.GeneratedCode;
using Keycloak.Common.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Refit;

namespace Stock.API.Client;

public static class ServicesExtensions
{
    public static IServiceCollection ConfigureRefitClients(this IServiceCollection services, Uri baseUrl)
    {
        services
            .AddRefitClient<ICacheApi>()
            .ConfigureHttpClient(c => c.BaseAddress = baseUrl)
            .AddHttpMessageHandler<ClientCredentialAuthHandler>()
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        Backoff.DecorrelatedJitterBackoffV2(
                            TimeSpan.FromSeconds(0.5),
                            3)));

        services
            .AddRefitClient<IStockApi>()
            .ConfigureHttpClient(c => c.BaseAddress = baseUrl)
            .AddHttpMessageHandler<ClientCredentialAuthHandler>()
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        Backoff.DecorrelatedJitterBackoffV2(
                            TimeSpan.FromSeconds(0.5),
                            3)));
        
        return services;
    }
}