using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly.Extensions.Http;
using Polly;

namespace HttpUtility.Extensions
{
    public static class IHttpClientBuilderExtensions
    {
        public static IHttpClientBuilder ConfigureRetryPolicy(this IHttpClientBuilder builder)
        {
            return builder.AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[] 
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4)
                }
            ));
        }
    }
}
