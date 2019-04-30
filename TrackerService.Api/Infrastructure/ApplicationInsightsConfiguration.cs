using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TrackerService.Api.Infrastructure
{
    public static class ApplicationInsightsConfiguration
    {
        public static void AddApplicationInsights(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                services.AddApplicationInsightsTelemetry(config.GetValue<string>("APPINSIGHTKEY"));
            }
        }
    }
}
