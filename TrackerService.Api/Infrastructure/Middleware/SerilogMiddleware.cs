using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using TrackerService.Api.Infrastructure.Logging;
using TrackerService.Common.Contracts;

namespace TrackerService.Api.Infrastructure.Middleware
{
    public class SerilogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostingEnvironment env;
        private readonly IServiceContext serviceContext;

        public SerilogMiddleware(RequestDelegate next, IHostingEnvironment env, IServiceContext serviceContext)
        {
            this.next = next;
            this.env = env;
            this.serviceContext = serviceContext;
        }

        public async Task Invoke(HttpContext context)
        {
            var valueProvider = new LoggingValueProvider(env, context);
            using (LogContext.Push(new LogEventEnricher(valueProvider, serviceContext)))
            {
                await next.Invoke(context);
            }
        }
    }
}
