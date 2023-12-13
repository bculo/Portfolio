using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using MediatR;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using User.Application.Features;
using User.Application.Interfaces;
using User.Functions.Extensions;
using User.Functions.Options;
using User.Functions.Services;

namespace User.Functions.Functions
{
    public class UserFunctions
    {
        [Function("user-info")]
        [OpenApiOperation(operationId: "user-info", tags: new[] { "User" })]
        [OpenApiSecurity("implicit_auth", SecuritySchemeType.OAuth2, Flows = typeof(ImplicitAuthFlow))]
        [OpenApiParameter(name: "userId", Required = true, In = ParameterLocation.Path)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", 
            bodyType: typeof(GetUserDetailsResponseDto), Description = "Get user information")]
        public async Task<HttpResponseData> GetUserInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var mediator = req.FunctionContext.InstanceServices.GetRequiredService<IMediator>();
            var userService = req.FunctionContext.InstanceServices.GetRequiredService<ICurrentUserService>();
            var serviceResponse = await mediator.Send(new GetUserDetailsDto { UserId = userService.GetUserId() });
            return await req.DefineResponse(HttpStatusCode.OK, serviceResponse);
        }
    }
}
