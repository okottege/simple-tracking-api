using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

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

                    if (env.IsDevelopment())
                    {
                        await context.Response.WriteAsync(exception.StackTrace);
                    }
                    
                    if(exception is HttpRequestException exReq)
                    {
                        
                    }
                    else
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    }

                    await context.Response.WriteAsync("There was an error");
                });
            });
        }
    }
}
