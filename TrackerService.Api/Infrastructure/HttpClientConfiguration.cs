using System;
using Microsoft.Extensions.DependencyInjection;
using TrackerService.Api.Infrastructure.Authentication;
using TrackerService.Api.Infrastructure.Contracts;
using TrackerService.Api.Infrastructure.Middleware;
using TrackerService.Common;

namespace TrackerService.Api.Infrastructure
{
    public static class HttpClientConfiguration
    {
        public static void AddHttpClients(
            this IServiceCollection services, AuthenticationConfig authConfig, UserManagementConfig userManagementConfig)
        {
            services.AddHttpClient(HttpClientNames.AUTHENTICATION_CLIENT, c =>
            {
                c.BaseAddress = new Uri(authConfig.Authority);
            });

            services.AddTransient<IServiceAuthenticator, ServiceToServiceAuthenticator>();

            var serviceProvider = services.BuildServiceProvider();
            var serviceAuth = serviceProvider.GetRequiredService<IServiceAuthenticator>();

            services.AddTransient<UserManagerErrorResponseHandler>();
            services.AddHttpClient(HttpClientNames.USER_MANAGEMENT_CLIENT, async http =>
                {
                    var token = await serviceAuth.AuthenticateAsync();
                    http.BaseAddress = new Uri(userManagementConfig.BaseUrl);
                    http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                })
                .AddHttpMessageHandler<UserManagerErrorResponseHandler>();
        }
    }
}
