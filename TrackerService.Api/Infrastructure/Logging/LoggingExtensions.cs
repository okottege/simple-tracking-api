using Serilog.Core;
using Serilog.Events;

namespace TrackerService.Api.Infrastructure.Logging
{
    public static class LoggingExtensions
    {
        public static void AddProperty(
            this (LogEvent logEvent, ILogEventPropertyFactory propFactory) logging, 
            string name, object value)
        {
            var property = logging.propFactory.CreateProperty(name, value);
            logging.logEvent.AddPropertyIfAbsent(property);
        }
    }
}
