using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
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
        private readonly ICurrentUserService _userService;
        private readonly IRegisterUserService _registrationService;

        public RegistrationFunctions(IRegisterUserService registrationService, 
            ICurrentUserService userService)
        {
            _userService = userService;
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
            var bodyInstanceJson = await req.ReadAsStringAsync();
            if (string.IsNullOrEmpty(bodyInstanceJson))
            {
                return await req.DefineResponse(HttpStatusCode.BadRequest, "Instance not provided in request body");
            }

            var instance = JsonConvert.DeserializeObject<CreateUserDto>(bodyInstanceJson);
            if(instance is null)
            {
                return await req.DefineResponse(HttpStatusCode.BadRequest, "Invalid instance provided in request body");
            }

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
