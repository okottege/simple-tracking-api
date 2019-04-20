using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TrackerService.Api.Infrastructure.Logging
{
    public class LoggingValueProvider
    {
        private readonly IHostingEnvironment environment;
        private readonly HttpContext httpContext;

        public LoggingValueProvider(IHostingEnvironment environment, HttpContext httpContext)
        {
            this.environment = environment;
            this.httpContext = httpContext;
        }

        public string ServiceName => AppDomain.CurrentDomain.FriendlyName;
        public string HostName => Dns.GetHostName();
        public string Environment => environment.EnvironmentName;
        public string TraceIdentifier => httpContext.TraceIdentifier;
        public IPAddress LocalIPAddress => httpContext.Connection.LocalIpAddress;
        public IPAddress RemoteIPAddress => httpContext.Connection.RemoteIpAddress;
    }
}
