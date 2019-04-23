using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace TrackerService.Api.Infrastructure.Middleware
{
    public class RequestIdGenerationMiddleware
    {
        private readonly RequestDelegate next;

        public RequestIdGenerationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();

            context.Request.Headers.TryGetValue(CustomHeaders.RequestId, out var requestIdCollection);
            if (requestIdCollection.Any())
            {
                requestId = requestIdCollection.First();
            }

            context.Response.Headers.Add(CustomHeaders.RequestId, requestId);
            context.Items.Add(CustomHeaders.RequestId, requestId);

            await next(context);
        }
    }
}
