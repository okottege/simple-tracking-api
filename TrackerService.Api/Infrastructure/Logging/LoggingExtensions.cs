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
            var property = CreateProperty(logging, name, value);
            logging.logEvent.AddPropertyIfAbsent(property);
        }

        public static void AddOrUpdateProperty(this (LogEvent logEvent, ILogEventPropertyFactory propFactory) logging,
            string name, object value)
        {
            var property = CreateProperty(logging, name, value);
            logging.logEvent.AddOrUpdateProperty(property);
        }

        private static LogEventProperty CreateProperty(
            (LogEvent _, ILogEventPropertyFactory propFactory) logging, 
            string name,
            object value)
        {
            return logging.propFactory.CreateProperty(name, value);
        }
    }
}
