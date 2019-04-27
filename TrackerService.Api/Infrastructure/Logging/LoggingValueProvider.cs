using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TrackerService.Common.Contracts;

namespace TrackerService.Api.Infrastructure.Logging
{
    public class LoggingValueProvider
    {
        private readonly IHostingEnvironment environment;
        private readonly HttpContext httpContext;
        private readonly IServiceContext serviceContext;

        public LoggingValueProvider(IHostingEnvironment environment, HttpContext httpContext, IServiceContext serviceContext)
        {
            this.environment = environment;
            this.httpContext = httpContext;
            this.serviceContext = serviceContext;
        }

        public string ServiceName => AppDomain.CurrentDomain.FriendlyName;
        public string RequestId => serviceContext.RequestId;
        public string RequestMethod => httpContext.Request.Method.ToUpperInvariant();
        public string HostName => Dns.GetHostName();
        public string Environment => environment.EnvironmentName;
        public string TraceIdentifier => httpContext.TraceIdentifier;
        public IPAddress LocalIPAddress => httpContext.Connection.LocalIpAddress;
        public IPAddress RemoteIPAddress => httpContext.Connection.RemoteIpAddress;
    }
}
