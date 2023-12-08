using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Core.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using User.Functions.Models;
using User.Functions.Utilities;

namespace User.Functions.Extensions
{
    public static class HttpRequestDataExtensions
    {
        public static async Task DefineResponseMiddleware(this HttpRequestData request, HttpStatusCode statusCode, 
            object message, bool isValidationError = false)
        {
            var responseBody = FormResponseBody(message, isValidationError);
            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(responseBody, SerializerUtilities.Create(), statusCode);
            request.FunctionContext.GetInvocationResult().Value = response;
        }

        public static async Task<HttpResponseData> DefineResponse<T>(this HttpRequestData request, 
            HttpStatusCode statusCode, T message, bool isValidationError = false) where T : notnull
        {
            var responseBody = FormResponseBody(message, isValidationError);
            var response = request.CreateResponse();
            await response.WriteAsJsonAsync(responseBody, SerializerUtilities.Create(), statusCode);
            return response;
        }
        
        private static object FormResponseBody(object bodyData, bool isValidationFailure)
        {
            if (!isValidationFailure)
            {
                return new FailureResponse
                {
                    Message = bodyData
                };
            }

            return new FailureValidationResponse
            {
                ValidationDict = bodyData
            };
        }
    }
}
