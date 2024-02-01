using Keycloak.Common.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Refit;
using Trend.API.Client.Services;

namespace Trend.API.Client;

public static class ServicesExtensions
{
    public static void ConfigureRefitClients(this IServiceCollection services, Uri baseUrl)
    {
        var clientBuilderINewsApi = services
            .AddRefitClient<INewsApi>()
            .AddHttpMessageHandler<ClientCredentialAuthHandler>()
            .ConfigureHttpClient(c => c.BaseAddress = baseUrl)
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        Backoff.DecorrelatedJitterBackoffV2(
                            TimeSpan.FromSeconds(0.5),
                            3)));

        var clientBuilderISearchWordApi = services
            .AddRefitClient<ISearchWordApi>()
            .AddHttpMessageHandler<ClientCredentialAuthHandler>()
            .ConfigureHttpClient(c => c.BaseAddress = baseUrl)
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        Backoff.DecorrelatedJitterBackoffV2(
                            TimeSpan.FromSeconds(0.5),
                            3)));

        var clientBuilderISyncApi = services
            .AddRefitClient<ISyncApi>()
            .AddHttpMessageHandler<ClientCredentialAuthHandler>()
            .ConfigureHttpClient(c => c.BaseAddress = baseUrl)
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        Backoff.DecorrelatedJitterBackoffV2(
                            TimeSpan.FromSeconds(0.5),
                            3)));
    }
}