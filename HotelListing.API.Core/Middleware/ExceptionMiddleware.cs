using HotelListingAPI.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace HotelListingAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            //Global Exception for every request
            try
            {
                //Await the result of the next opretion relative to request
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Something went wrong while processing {nameof(context.Request.Path)}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //We handling by some info

            context.Response.ContentType = "appiclation/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            var errorDetails = new ErrorDetails
            {
                ErrorType = "Failure",
                ErrorMessage = ex.Message,
            };

            switch (ex)
            {
                case NotFoundException notFoundException:

                    statusCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "Not Found";
                    break;
                default:
                    break;
            }

            //Convert to json string and response with the status
            string response = JsonConvert.SerializeObject(errorDetails);
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(response);
        }
    }

    public class ErrorDetails
    {
        public string ErrorType { get; set; }

        public string ErrorMessage { get; set; }

    }
}
