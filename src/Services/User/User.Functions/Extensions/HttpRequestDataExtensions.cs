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
        public static async Task DefineResponse(this HttpRequestData request, HttpStatusCode statusCode, string message)
        {
            var responseInfo = new FailureResponse
            {
                Message = message,
            };

            var response = request.CreateResponse(statusCode);
            await response.WriteAsJsonAsync(responseInfo);

            request.FunctionContext.GetInvocationResult().Value = response;
        }
    }
}
