using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using User.Application.Interfaces;
using User.Functions.Extensions;
using User.Functions.Options;
using User.Functions.Services;

namespace User.Functions.Functions
{
    public class UserFunctions
    {
        private readonly IUserManagerService _userService;
        private readonly ICurrentUserService _currentUser;

        public UserFunctions(IUserManagerService userService,
            ICurrentUserService currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        [Function("UserInfo")]
        [OpenApiOperation(operationId: "GetUserInfo", tags: new[] { "User" })]
        [OpenApiSecurity("implicit_auth", SecuritySchemeType.OAuth2, Flows = typeof(ImplicitAuthFlow))]
        [OpenApiParameter(name: "userId", Required = true, In = ParameterLocation.Path)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", 
            bodyType: typeof(UserDetailsDto), Description = "Get user information")]
        public async Task<HttpResponseData> GetUserInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var serviceResponse = await _userService.GetUserDetails(_currentUser.GetUserId());
            return await req.DefineResponse(HttpStatusCode.OK, serviceResponse);
        }
    }
}
