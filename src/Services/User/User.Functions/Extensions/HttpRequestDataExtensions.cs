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
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using User.Application.Common.Exceptions;
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

        public static async Task<T> ToDto<T>(this HttpRequestData request,
            CancellationToken token = default,
            string bodyIsNullMsg = "Instance not provided in request body",
            string deserializationErrorMsg = "Invalid instance provided in request body") where T : class
        {
            var bodyInstanceJson = await request.ReadAsStringAsync()
                .ConfigureAwait(false);
            if (string.IsNullOrEmpty(bodyInstanceJson))
            {
                throw new PortfolioUserCoreException(bodyIsNullMsg, bodyIsNullMsg);
            }

            var instance = JsonConvert.DeserializeObject<T>(bodyInstanceJson);
            if(instance is null)
            {
                throw new PortfolioUserCoreException(deserializationErrorMsg, deserializationErrorMsg);
            }

            return instance;
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
