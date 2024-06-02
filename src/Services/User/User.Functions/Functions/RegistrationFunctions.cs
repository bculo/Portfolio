using System.Net;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using User.Application.Features;
using User.Functions.Extensions;
using User.Functions.Options;

namespace User.Functions.Functions
{
    public class RegistrationFunctions
    {
        [Function("register-user")]
        [OpenApiOperation(operationId: "register-user", tags: new[] { "Manage" })]
        [OpenApiSecurity("implicit_auth", SecuritySchemeType.OAuth2, Flows = typeof(ImplicitAuthFlow))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(RegisterUserDto), Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
        public async Task<HttpResponseData> RegisterUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, CancellationToken token)
        {
            var mediator = req.FunctionContext.InstanceServices.GetRequiredService<IMediator>();
            var dto = await req.ToDto<RegisterUserDto>(token);
            await mediator.Send(dto, token);  
            return req.CreateResponse(HttpStatusCode.NoContent);
        }

        [Function("approve-user")]
        [OpenApiOperation(operationId: "approve-user", tags: new[] { "Manage" })]
        [OpenApiSecurity("implicit_auth", SecuritySchemeType.OAuth2, Flows = typeof(ImplicitAuthFlow))]
        [OpenApiParameter(name: "userName", Required = true, In = ParameterLocation.Path)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
        public async Task<HttpResponseData> ApproveUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "approve-user/{userName}")]
            HttpRequestData req, 
            string userName,
            CancellationToken token)
        {
            var mediator = req.FunctionContext.InstanceServices.GetRequiredService<IMediator>();
            await mediator.Send(new VerifyUserDto { UserName = userName }, token);  
            return req.CreateResponse(HttpStatusCode.NoContent);
        }
        
        [Function("upload-verification-image")]
        [OpenApiOperation(operationId: "upload-verification-image", tags: new[] { "Manage" })]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(UploadVerificationImageFormData), Required = true)]
        [OpenApiSecurity("implicit_auth", SecuritySchemeType.OAuth2, Flows = typeof(ImplicitAuthFlow))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
        public async Task<HttpResponseData> UploadVerificationImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequestData req,
            CancellationToken token)
        {
            var file = req.GetFileFromRequest("Image");
            var mediator = req.FunctionContext.InstanceServices.GetRequiredService<IMediator>();
            await mediator.Send(new UploadVerificationImageDto
            {
                Name = file.FileName,
                ContentType = file.ContentType,
                Image = await file.ToBytes()
            }, token);  
            return req.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
