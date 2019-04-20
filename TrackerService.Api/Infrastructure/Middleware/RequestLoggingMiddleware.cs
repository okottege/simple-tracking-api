using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TrackerService.Api.Infrastructure.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestLoggingMiddleware> logger;
        private static readonly string[] requestPathsShouldNotLog = {"/health", "/ready", "/metrics"};

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (ShouldLogRequest(context.Request.Path))
            {
                logger.LogInformation("Request started.");
                var startTime = DateTime.Now;
                var durationMS = 0D;
                try
                {
                    await next.Invoke(context);
                    durationMS = (DateTime.Now - startTime).TotalMilliseconds;
                }
                finally
                {
                    logger.LogInformation($"Request completed with {context.Response.StatusCode} in {durationMS} (ms).");
                }
            }
            else
            {
                await next.Invoke(context);
            }
        }

        private bool ShouldLogRequest(string reqPath) => !requestPathsShouldNotLog.Contains(reqPath);
    }
}
