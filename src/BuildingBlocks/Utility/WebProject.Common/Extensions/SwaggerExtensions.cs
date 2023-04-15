using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Common.Options;

namespace WebProject.Common.Extensions
{
    public static class SwaggerExtensions
    {
        public static void ConfigureSwaggerWithApiVersioning(this IServiceCollection services,
            string applicationName,
            string authorizationUrl,
            int majorApiVersion,
            int minorApiVersion)
        {

            services.AddApiVersioning(config =>
            {
                var majorVersion = majorApiVersion;
                var minorVersion = minorApiVersion;
                config.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authorizationUrl),
                        }
                    },
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddOptions<ServiceSwaggerOptions>().Configure(opt =>
            {
                opt.ApplicationName = applicationName ?? "Unknown service";
            });

            services.ConfigureOptions<ConfigureSwaggerOptions>();
        }

        internal class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
        {
            private readonly IApiVersionDescriptionProvider _provider;
            private readonly ServiceSwaggerOptions _options;

            public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider,
                IOptions<ServiceSwaggerOptions> options)
            {
                _provider = provider;
                _options = options.Value;
            }

            public void Configure(string name, SwaggerGenOptions options)
            {
                Configure(options);
            }

            public void Configure(SwaggerGenOptions options)
            {
                foreach (var description in _provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
                }
            }

            private OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
            {
                var info = new OpenApiInfo()
                {
                    Title = _options.ApplicationName,
                    Version = desc.ApiVersion.ToString()
                };

                if (desc.IsDeprecated)
                {
                    info.Description += "This API version has been deprecated. Please use one of the new APIs available from the explorer.";
                }

                return info;
            }
        }
    }
}
