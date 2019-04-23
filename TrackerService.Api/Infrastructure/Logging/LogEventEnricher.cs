using System;
using Serilog.Core;
using Serilog.Events;
using TrackerService.Common.Contracts;

namespace TrackerService.Api.Infrastructure.Logging
{
    public class LogEventEnricher : ILogEventEnricher
    {
        private readonly LoggingValueProvider valueProvider;
        private readonly IServiceContext serviceContext;

        public LogEventEnricher(LoggingValueProvider valueProvider, IServiceContext serviceContext)
        {
            this.valueProvider = valueProvider;
            this.serviceContext = serviceContext;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var context = (logEvent, propertyFactory);

            context.AddProperty("format", "2");
            context.AddProperty("service", valueProvider.ServiceName);
            context.AddProperty("correlationId", serviceContext.RequestId);
            context.AddProperty("hostName", valueProvider.HostName);
            context.AddProperty("environment", valueProvider.Environment);
            context.AddProperty("timestamp", logEvent.Timestamp.ToUnixTimeMilliseconds());
            context.AddProperty("timestampISO", logEvent.Timestamp.ToOffset(TimeSpan.Zero));
            context.AddProperty("message", logEvent.MessageTemplate);
            context.AddProperty("traceIdentifier", valueProvider.TraceIdentifier);
            context.AddProperty("levelTag", logEvent.Level.ToString());
            context.AddProperty("localIPAddress", valueProvider.LocalIPAddress);
            context.AddProperty("remoteIPAddress", valueProvider.RemoteIPAddress);
        }
    }
}
