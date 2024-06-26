﻿using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
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

        private static HttpContext AsHttpContext(this HttpRequestData req)
        {
            var httpContext = new DefaultHttpContext
            {
                Request =
                {
                    Method = req.Method,
                    Path = PathString.FromUriComponent(req.Url),
                    Host = HostString.FromUriComponent(req.Url),
                    Scheme = req.Url.Scheme,
                    Query = new QueryCollection(QueryHelpers.ParseQuery(req.Query.ToString()))
                }
            };
            foreach (var header in req.Headers)
                httpContext.Request.Headers[header.Key] = header.Value.ToArray();
            httpContext.Request.Body = req.Body;
            return httpContext;
        }

        public static IFormFile GetFileFromRequest(this HttpRequestData req, string key)
        {
            var files = req.AsHttpContext().Request.Form.Files;
            if (!files.Any())
            {
                throw new ArgumentNullException(nameof(files));
            }

            var wantedFile = files.GetFile(key);
            if (wantedFile is null)
            {
                throw new ArgumentNullException(nameof(wantedFile));
            }

            
            return wantedFile;
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
