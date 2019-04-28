using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace TrackerService.Api.Infrastructure.HealthChecks
{
    public static class HealthCheckExtensions
    {
        private const string ApiHealthCheckName = "api-health-check";

        public static void AddApiHealthCheck(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<ApiHealthCheck>(ApiHealthCheckName);
        }

        public static void UseApiHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    var apiHealthReport = report.Entries[ApiHealthCheckName];
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = apiHealthReport.Status == HealthStatus.Healthy
                        ? (int)HttpStatusCode.OK
                        : (int)HttpStatusCode.ServiceUnavailable;

                    var healthResponse = new HealthCheckResponse(apiHealthReport);
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(healthResponse, Formatting.Indented));
                }
            });
        }
    }
}
