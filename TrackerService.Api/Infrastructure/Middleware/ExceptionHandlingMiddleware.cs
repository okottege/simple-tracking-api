using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TrackerService.Api.CustomExceptions;
using TrackerService.Common.Exceptions;

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
                case EntityNotFoundException exNotFound:
                    await ApplyExceptionResponse(context, HttpStatusCode.NotFound, exNotFound.Message);
                    break;
                case ServiceAccessException exServiceAccess:
                    await HandleServiceAccessException(context, exServiceAccess);
                    break;
                default:
                    await ApplyExceptionResponse(context, HttpStatusCode.InternalServerError, "There was an error processing request.");
                    break;
            }
        }

        private static async Task ApplyExceptionResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int) statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message }));
        }

        private static async Task HandleServiceAccessException(HttpContext context, ServiceAccessException ex)
        {
            var msg = await ex.Response.Content.ReadAsStringAsync();
            await ApplyExceptionResponse(context, ex.Response.StatusCode, msg);
        }
    }
}
