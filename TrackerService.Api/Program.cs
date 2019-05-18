using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using TrackerService.Api.Infrastructure.Logging;

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
                        else
                        {
                            var keyVaultEndpoint = hostingContext.Configuration.GetValue<string>("KEYVAULTENDPOINT");
                            SetupAzureKeyVault(keyVaultEndpoint, config);
                        }
                    })
                .UseSerilog(SetupSerilog)
                .UseStartup<Startup>();

        private static void SetupSerilog(WebHostBuilderContext context, LoggerConfiguration config)
        {
            var appInsightKey = context.Configuration.GetValue<string>("APPINSIGHTKEY");
            config.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                  .Enrich.FromLogContext()
                  .Enrich.With<RemoveLogPropertiesEnricher>()
                  .WriteTo.Console(new RenderedCompactJsonFormatter())
                  .WriteTo.ApplicationInsights(appInsightKey, TelemetryConverter.Traces, LogEventLevel.Information);
        }

        private static void SetupAzureKeyVault(string endpoint, IConfigurationBuilder builder)
        {
            if (string.IsNullOrWhiteSpace(endpoint)) return;

            var tokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));
            builder.AddAzureKeyVault(endpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
        }
    }
}
