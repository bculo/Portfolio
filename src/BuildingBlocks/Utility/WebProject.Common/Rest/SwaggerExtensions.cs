﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebProject.Common.Options;

namespace WebProject.Common.Rest;

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
                    []
                }
            });

            opt.SupportNonNullableReferenceTypes();
            opt.UseAllOfToExtendReferenceSchemas();
            opt.UseOneOfForPolymorphism();
            opt.UseAllOfForInheritance();
            opt.SchemaFilter<GenericFilter>();
            opt.EnableAnnotations();
        });
        
        services.AddOptions<ServiceSwaggerOptions>().Configure(opt =>
        {
            opt.ApplicationName = applicationName ?? "Unknown service";
        });

        services.ConfigureOptions<ConfigureSwaggerApiVersioningOptions>();
    }

    public static void ConfigureSwaggerDefault(this IServiceCollection services, string authorizationUrl)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityConfiguration(authorizationUrl);
        });
    }
    public static void ConfigureSwaggerAsEndpoints(this IServiceCollection services, string authorizationUrl)
    {
        services.AddSwaggerGen(c =>
        {
            c.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                {
                    return new[] { api.GroupName };
                }

                if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    return new[] { controllerActionDescriptor.ControllerName };
                }

                throw new InvalidOperationException("Unable to determine tag for endpoint.");
            });
            
            c.DocInclusionPredicate((name, api) => true);
            c.AddSecurityConfiguration(authorizationUrl);
        });
    }

    private static void AddSecurityConfiguration(this SwaggerGenOptions options, string authorizationUrl)
    {
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
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

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                []
            }
        });
    }
    
    internal class ConfigureSwaggerApiVersioningOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly ServiceSwaggerOptions _options;

        public ConfigureSwaggerApiVersioningOptions(IApiVersionDescriptionProvider provider,
            IOptions<ServiceSwaggerOptions> options)
        {
            _provider = provider;
            _options = options.Value;
        }

        public void Configure(string? name, SwaggerGenOptions options)
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

    private class GenericFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            if (type.IsGenericType == false)
                return;

            schema.Title = $"{type.Name[0..^2]}<{type.GenericTypeArguments[0].Name}>";
        }
    }
}

