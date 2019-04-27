using System;
using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TrackerService.Api.Infrastructure
{
    public class HealthCheckResponse
    {
        public HealthCheckResponse(HealthReportEntry report)
        {
            Status = report.Status.ToString();

            UserManagement = GetStatus("UserManagement", report.Data);
            DatabaseConnectivity = GetStatus("Database", report.Data);
        }

        public string Status { get; }
        public string UserManagement { get; }
        public string DatabaseConnectivity { get; }

        private static string GetStatus(string field, IReadOnlyDictionary<string, object> entries)
        {
            return Convert.ToBoolean(entries[field]) ? "Ok" : "Fail";
        }
    }
}
