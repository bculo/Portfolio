using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using User.Application.Features;
using User.Functions.Extensions;
using User.Functions.Options;

namespace User.Functions.Functions;

public class LogoutFunctions
{
    [Function("sso-logout")]
    [OpenApiOperation(operationId: "sso-logout", tags: new[] { "Session" })]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
    public HttpResponseData RegisterUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", "get")] HttpRequestData req, CancellationToken token)
    {
        return req.CreateResponse(HttpStatusCode.NoContent);
    }
}