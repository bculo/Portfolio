using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using User.Functions.Models;

namespace User.Functions.Extensions
{
    public static class HttpRequestDataExtensions
    {
        public static async Task DefineResponseMiddleware(this HttpRequestData request, HttpStatusCode statusCode, string message)
        {
            var responseInfo = new FailureResponse
            {
                Message = message,
            };

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(responseInfo, statusCode: statusCode);

            request.FunctionContext.GetInvocationResult().Value = response;
        }

        public static async Task<HttpResponseData> DefineResponse(this HttpRequestData request, HttpStatusCode statusCode, string message)
        {
            var responseInfo = new FailureResponse
            {
                Message = message,
            };

            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(responseInfo, statusCode: statusCode);

            return response;
        }
    }
}
