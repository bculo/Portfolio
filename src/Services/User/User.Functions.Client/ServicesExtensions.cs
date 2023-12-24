﻿using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Refit;
using User.Functions.Client.Services;

namespace User.Functions.Client;

public static class ServicesExtensions
{
    public static void ConfigureRefitClients(this IServiceCollection services, Uri baseUrl)
    {
        var clientBuilderISessionApi = services
            .AddRefitClient<ISessionApi>()
            .ConfigureHttpClient(c => c.BaseAddress = baseUrl)
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        Backoff.DecorrelatedJitterBackoffV2(
                            TimeSpan.FromSeconds(0.5),
                            3)));

        var clientBuilderIManageApi = services
            .AddRefitClient<IManageApi>()
            .ConfigureHttpClient(c => c.BaseAddress = baseUrl)
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        Backoff.DecorrelatedJitterBackoffV2(
                            TimeSpan.FromSeconds(0.5),
                            3)));

        var clientBuilderIUserApi = services
            .AddRefitClient<IUserApi>()
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