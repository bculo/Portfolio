using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using User.Application.Interfaces;
using User.Functions.Extensions;
using User.Functions.Models;
using User.Functions.Options;
using User.Functions.Services;

namespace User.Functions.Functions
{
    public class RegistrationFunctions
    {
        private readonly IRegisterUserService _registrationService;

        public RegistrationFunctions(IRegisterUserService registrationService)
        {
            _registrationService = registrationService;
        }

        [Function("RegisterUser")]
        [OpenApiOperation(operationId: "RegisterUser", tags: new[] { "Manage" })]
        [OpenApiSecurity("implicit_auth", SecuritySchemeType.OAuth2, Flows = typeof(ImplicitAuthFlow))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateUserDto), Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
        public async Task<HttpResponseData> RegisterUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, CancellationToken token)
        {
            var validator =
                req.FunctionContext.InstanceServices.GetService(typeof(IValidator<CreateUserDto>)) as
                    IValidator<CreateUserDto>;
            var instance = await req.ToDto(validator, token);
            await _registrationService.RegisterUser(instance, token);
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
            await _registrationService.ApproveUser(userId, token);
            return req.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
