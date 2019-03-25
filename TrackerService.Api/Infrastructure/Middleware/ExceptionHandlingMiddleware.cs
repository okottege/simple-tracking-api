using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TrackerService.Api.CustomExceptions;

namespace TrackerService.Api.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            switch (ex)
            {
                case UserManagementException exUserManagement:
                    await ApplyExceptionResponse(context, HttpStatusCode.BadRequest, exUserManagement.Message);
                    break;
                default:
                    await ApplyExceptionResponse(context, HttpStatusCode.InternalServerError, "There was an error processing request.");
                    break;
            }
        }

        private static Task ApplyExceptionResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int) statusCode;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new { message }));
        }
    }
}
