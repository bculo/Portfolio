using System.Collections.Frozen;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using User.Application.Interfaces;
using User.Functions.Extensions;
using User.Functions.Services;

namespace User.Functions.Middlewares
{
    public class AuthorizationMiddleware(ILogger<AuthorizationMiddleware> logger, IServiceProvider provider)
        : IFunctionsWorkerMiddleware
    {
        private readonly IServiceProvider _provider = provider;

        private readonly FrozenSet<string> _whiteList = new List<string>
        {
            "RenderOAuth2Redirect",
            "RenderOpenApiDocument",
            "RenderSwaggerDocument",
            "RenderSwaggerUI",
            "sso-logout"
        }.ToFrozenSet();

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var functionName = context.FunctionDefinition.Name;
            if (_whiteList.Contains(functionName))
            {
                await next(context);
                return;
            }
            
            var requestData = await context.GetHttpRequestDataAsync();
            var bearerToken = GetAuthorizationToken(requestData);
            if(bearerToken is null)
            {
                await requestData.DefineResponseMiddleware(HttpStatusCode.Unauthorized, "Authorization token not provided");
                return;
            }

            var tokenService = context.InstanceServices.GetService(typeof(ITokenService)) as ITokenService;
            if (tokenService is null)
            {
                logger.LogCritical("ITokenService not registered via DI!!!");
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
                logger.LogCritical("IUserService not registered via DI!!!");
                await requestData.DefineResponseMiddleware(HttpStatusCode.InternalServerError, "Service not available!");
                return;
            }

            userService.InitializeUser(tokenValidationResult.Claims);

            await next(context);
        }

        private string? GetAuthorizationToken(HttpRequestData request)
        {
            return request.Headers.TryGetValues("Authorization", out var bearerTokens) 
                ? bearerTokens.FirstOrDefault() 
                : null;
        }

        private string RemoveBearerPrefix(string bearerToken)
        {
            return bearerToken.StartsWith("Bearer ") 
                ? bearerToken.Replace("Bearer ", "") 
                : bearerToken;
        }
    }
}
