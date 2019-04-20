using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog.Context;
using TrackerService.Api.Infrastructure.Logging;

namespace TrackerService.Api.Infrastructure.Middleware
{
    public class SerilogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostingEnvironment env;
        private readonly IConfiguration config;

        public SerilogMiddleware(RequestDelegate next, IHostingEnvironment env, IConfiguration config)
        {
            this.next = next;
            this.env = env;
            this.config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            var valueProvider = new LoggingValueProvider(env, context);
            using (LogContext.Push(new LogEventEnricher(valueProvider)))
            {
                await next.Invoke(context);
            }
        }
    }
}
