using System.Net;
using MassTransit.Internals;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
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
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(AddNewUserDto), Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
        public async Task<HttpResponseData> RegisterUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, CancellationToken token)
        {
            var mediator = req.FunctionContext.InstanceServices.GetRequiredService<IMediator>();
            var dto = await req.ToDto<AddNewUserDto>(token);
            await mediator.Send(dto, token);  
            return req.CreateResponse(HttpStatusCode.NoContent);
        }

        [Function("approve-user")]
        [OpenApiOperation(operationId: "approve-user", tags: new[] { "Manage" })]
        [OpenApiSecurity("implicit_auth", SecuritySchemeType.OAuth2, Flows = typeof(ImplicitAuthFlow))]
        [OpenApiParameter(name: "userId", Required = true, In = ParameterLocation.Path)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
        public async Task<HttpResponseData> ApproveUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "approve-user/{userId}")]
            HttpRequestData req, 
            long userId,
            CancellationToken token)
        {
            var mediator = req.FunctionContext.InstanceServices.GetRequiredService<IMediator>();
            await mediator.Send(new ApproveNewUserDto { UserId = userId }, token);  
            return req.CreateResponse(HttpStatusCode.NoContent);
        }
        
        [Function("upload-image")]
        [OpenApiOperation(operationId: "upload-image", tags: new[] { "Manage" })]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(UploadVerificationImageFormData), Required = true)]
        [OpenApiSecurity("implicit_auth", SecuritySchemeType.OAuth2, Flows = typeof(ImplicitAuthFlow))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NoContent)]
        public async Task<HttpResponseData> UploadImage(
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
