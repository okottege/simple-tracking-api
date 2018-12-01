using Microsoft.AspNetCore.Builder;

namespace TrackerService.Api.Infrastructure.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApplicationVersionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApplicationVersionMiddleware>();
        }
    }
}
