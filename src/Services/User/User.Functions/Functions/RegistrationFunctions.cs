using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using User.Application.Interfaces;
using User.Functions.Extensions;
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
        public async Task<HttpResponseData> RegisterUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, CancellationToken token)
        {
            var bodyInstanceJson = req.ReadAsString();
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

        [Function("RegisterUser")]
        public async Task<HttpResponseData> ApproveUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
