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