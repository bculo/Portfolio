using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using User.Application.Features;
using User.Application.Interfaces;
using User.Functions.Extensions;
using User.Functions.Models;
using User.Functions.Options;
using User.Functions.Services;

namespace User.Functions.Functions
{
    public class RegistrationFunctions
    {
        [Function("RegisterUser")]
        [OpenApiOperation(operationId: "RegisterUser", tags: new[] { "Manage" })]
        [OpenApiSecurity("implicit_auth", SecuritySchemeType.OAuth2, Flows = typeof(ImplicitAuthFlow))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateUserDto), Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
        public async Task<HttpResponseData> RegisterUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, CancellationToken token)
        {
            var mediator = req.FunctionContext.InstanceServices.GetRequiredService<IMediator>();
            var dto = await req.ToDto<AddNewUserDto>(validator: null, token: token);
            await mediator.Send(dto, token);  
            return req.CreateResponse(HttpStatusCode.NoContent);
        }

        [Function("ApproveUser")]
        [OpenApiOperation(operationId: "ApproveUser", tags: new[] { "Manage" })]
        [OpenApiSecurity("implicit_auth", SecuritySchemeType.OAuth2, Flows = typeof(ImplicitAuthFlow))]
        [OpenApiParameter(name: "userId", Required = true, In = ParameterLocation.Path)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
        public async Task<HttpResponseData> ApproveUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ApproveUser/{userId}")]
            HttpRequestData req, 
            long userId,
            CancellationToken token)
        {
            //await _registrationService.ApproveUser(userId, token);
            return req.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
