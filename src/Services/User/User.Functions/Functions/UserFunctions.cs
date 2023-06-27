using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using User.Application.Interfaces;
using User.Functions.Extensions;
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
        public async Task<HttpResponseData> GetUserInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var serviceResponse = await _userService.GetUserDetails(_currentUser.GetUserId());
            return await req.DefineResponse(HttpStatusCode.OK, serviceResponse);
        }
    }
}
