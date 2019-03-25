using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tracker.Api.Tests.Integration.Application.Authentication;
using TrackerService.Api;
using TrackerService.Data.Contracts;

namespace Tracker.Api.Tests.Integration.Application
{
    public class InMemoryWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(".");
            base.ConfigureWebHost(builder);
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var configSettings = new Dictionary<string, string>
            {
                ["AuthenticationConfig:Authority"] = "https://localhost:8080/auth",
                ["AuthenticationConfig:Audience"] = "",
                ["AuthenticationConfig:ClientID"] = "abc-1234",
                ["AuthenticationConfig:ClientSecret"] = "abc-1234",
                ["AuthenticationConfig:Realm"] = "abc-1234",
                ["AuthenticationConfig:GrantType"] = "abc-1234",

                ["UserManagementConfig:ClientID"] = "user-man-1234",
                ["UserManagementConfig:BaseUrl"] = "https://localhost/user-management"
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configSettings)
                .Build();

            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseEnvironment("test")
                .ConfigureTestServices(ConfigureTestServices)
                .UseStartup<Startup>();
        }

        private static void ConfigureTestServices(IServiceCollection services)
        {
            services.AddAuthentication(TestAuthenticationExtensions.TestAuthScheme)
                .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(TestAuthenticationExtensions.TestAuthScheme, opt => { });
        }
    }

    internal static class InMemoryApplicationFactoryExtensions
    {
        public static T GetService<T>(this InMemoryWebApplicationFactory factory)
        {
            return factory.Server.Host.Services.GetService<T>();
        }
    }
}
