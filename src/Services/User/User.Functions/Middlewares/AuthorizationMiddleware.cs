using System.Collections.Frozen;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using User.Application.Interfaces;
using User.Functions.Extensions;
using User.Functions.Services;

namespace User.Functions.Middlewares
{
    public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<AuthorizationMiddleware> _logger;
        private readonly FrozenSet<string> _whiteList;

        public AuthorizationMiddleware(ILogger<AuthorizationMiddleware> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
            
            _whiteList = new List<string>
            {
                "RenderOAuth2Redirect",
                "RenderOpenApiDocument",
                "RenderSwaggerDocument",
                "RenderSwaggerUI",
                "sso-logout"
            }.ToFrozenSet();
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var functionName = context.FunctionDefinition.Name;
            if (_whiteList.Contains(functionName))
            {
                await next(context);
                return;
            }
            
            var requestData = await context.GetHttpRequestDataAsync();
            string bearerToken = GetAuthorizationToken(requestData);
            if(bearerToken is null)
            {
                await requestData.DefineResponseMiddleware(HttpStatusCode.Unauthorized, "Authorization token not provided");
                return;
            }

            var tokenService = context.InstanceServices.GetService(typeof(ITokenService)) as ITokenService;
            if (tokenService is null)
            {
                _logger.LogCritical("ITokenService not registered via DI!!!");
                await requestData.DefineResponseMiddleware(HttpStatusCode.InternalServerError, "Service not available!");
                return;
            }

            var tokenWithoutPrefix = RemoveBearerPrefix(bearerToken);
            var tokenValidationResult = await tokenService.Validate(tokenWithoutPrefix);
            if (!tokenValidationResult.IsValid)
            {
                await requestData.DefineResponseMiddleware(HttpStatusCode.Unauthorized, tokenValidationResult.FailureReason);
                return;
            }

            var userService = context.InstanceServices.GetService(typeof(ICurrentUserService)) as ICurrentUserService;
            if (userService is null)
            {
                _logger.LogCritical("IUserService not registered via DI!!!");
                await requestData.DefineResponseMiddleware(HttpStatusCode.InternalServerError, "Service not available!");
                return;
            }

            userService.InitializeUser(tokenValidationResult.Claims);

            await next(context);
        }

        private string GetAuthorizationToken(HttpRequestData request)
        {
            if (request.Headers.TryGetValues("Authorization", out var bearerTokens))
            {
                return bearerTokens.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private string RemoveBearerPrefix(string bearerToken)
        {
            if(bearerToken.StartsWith("Bearer ")) 
            {
                return bearerToken.Replace("Bearer ", "");
            }

            return bearerToken;
        }
    }
}
