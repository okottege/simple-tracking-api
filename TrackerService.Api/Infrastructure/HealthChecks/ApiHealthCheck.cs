using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TrackerService.Common;
using TrackerService.Data.Contracts;

namespace TrackerService.Api.Infrastructure.HealthChecks
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly IDBHealthCheckRepository dbHealthRepo;
        private readonly HttpClient http;

        public ApiHealthCheck(IRepositoryFactory repoFactory, IHttpClientFactory httpFactory)
        {
            dbHealthRepo = repoFactory.CreateDBHealthRepository();
            http = httpFactory.CreateClient(HttpClientNames.AUTHENTICATION_CLIENT);
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var userManagementResponse = await http.GetAsync("/test", cancellationToken);
            var isUserManagementOk = userManagementResponse.StatusCode == HttpStatusCode.OK;
            var canConnectToDb = await dbHealthRepo.CanConnectToDatabase();
            var healthStatus = new[] {isUserManagementOk, canConnectToDb}.All(v => v)
                ? HealthStatus.Healthy
                : HealthStatus.Unhealthy;
            var data = new Dictionary<string, object>
            {
                ["UserManagement"] = isUserManagementOk,
                ["Database"] = canConnectToDb
            };
            return new HealthCheckResult(healthStatus, "Api Health Status", null, new ReadOnlyDictionary<string, object>(data));
        }
    }
}
