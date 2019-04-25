using Serilog.Core;
using Serilog.Events;

namespace TrackerService.Api.Infrastructure.Logging
{
    public class RemoveLogPropertiesEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.RemovePropertyIfPresent("ActionId");
            logEvent.RemovePropertyIfPresent("CorrelationId");
        }
    }
}
