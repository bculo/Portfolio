using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using User.Functions.Services;

namespace User.Functions
{
    public class Functions
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public Functions(ILoggerFactory loggerFactory, IUserService userService)
        {
            _logger = loggerFactory.CreateLogger<Functions>();
            _userService = userService;
        }

        [Function("RegisterUser")]
        public HttpResponseData RegisterUser([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("RegisterUser method invoked!!!");
            return response;
        }

        [Function("UserInfo")]
        public HttpResponseData GetUserInfo([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString($"UserInfo method invoked for user {_userService.GetUserId()}");
            return response;
        }
    }
}
