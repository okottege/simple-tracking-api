using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace TrackerService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        if (hostingContext.HostingEnvironment.IsDevelopment())
                        {
                            config.SetBasePath(Directory.GetCurrentDirectory());
                            config.AddJsonFile("appSettings.dev.json", true);
                        }
                    })
                .UseSerilog(SetupSerilog)
                .UseStartup<Startup>();

        private static void SetupSerilog(WebHostBuilderContext context, LoggerConfiguration config)
        {
            var appInsightKey = context.Configuration.GetValue<string>("APPINSIGHTKEY");
            config.Enrich.FromLogContext()
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.ApplicationInsights(appInsightKey, TelemetryConverter.Traces, LogEventLevel.Warning);
        }
    }
}
