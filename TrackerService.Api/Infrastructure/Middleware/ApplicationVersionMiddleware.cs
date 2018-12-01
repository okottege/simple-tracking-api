using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TrackerService.Api.Infrastructure.Middleware
{
    public class ApplicationVersionMiddleware
    {
        private readonly RequestDelegate next;

        public ApplicationVersionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var version = Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
            context.Response.Headers["x-track-appversion"] = version;
            await next.Invoke(context);
        }
    }
}
