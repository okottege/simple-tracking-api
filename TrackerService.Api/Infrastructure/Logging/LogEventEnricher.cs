using System;
using Serilog.Core;
using Serilog.Events;

namespace TrackerService.Api.Infrastructure.Logging
{
    public class LogEventEnricher : ILogEventEnricher
    {
        private readonly LoggingValueProvider valueProvider;
        public LogEventEnricher(LoggingValueProvider valueProvider)
        {
            this.valueProvider = valueProvider;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var context = (logEvent, propertyFactory);

            context.AddProperty("service", valueProvider.ServiceName);

            context.AddOrUpdateProperty("RequestId", valueProvider.RequestId);
            context.AddProperty("hostName", valueProvider.HostName);
            context.AddProperty("requestMethod", valueProvider.RequestMethod);
            context.AddProperty("environment", valueProvider.Environment);
            context.AddProperty("timestamp", logEvent.Timestamp.ToUnixTimeMilliseconds());
            context.AddProperty("timestampISO", logEvent.Timestamp.ToOffset(TimeSpan.Zero));
            context.AddProperty("message", logEvent.MessageTemplate);
            context.AddProperty("traceIdentifier", valueProvider.TraceIdentifier);
            context.AddProperty("levelTag", logEvent.Level.ToString());
            context.AddProperty("localIPAddress", valueProvider.LocalIPAddress);
            context.AddProperty("remoteIPAddress", valueProvider.RemoteIPAddress);
            context.AddProperty("statusCode", valueProvider.StatusCode);
        }
    }
}
