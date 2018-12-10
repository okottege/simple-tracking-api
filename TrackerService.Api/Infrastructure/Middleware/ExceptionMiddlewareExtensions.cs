using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TrackerService.Api.CustomExceptions;

namespace TrackerService.Api.Infrastructure.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature?.Error;

                    if(exception is UserManagementException exUserManagement)
                    {
                        context.Response.StatusCode = (int) exUserManagement.Status;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(exUserManagement.Content);
                    }
                    else
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("There was an error");
                    }
                });
            });
        }
    }
}
