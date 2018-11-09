﻿using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Tracker.Api.Tests.Integration.Application.Authentication;
using TrackerService.Data.Contracts;

namespace Tracker.Api.Tests.Integration.Application
{
    public class InMemoryWebApplicationFactory<TStartup> : WebApplicationFactory<TestStartup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var mockRepoFactory = new Mock<IRepositoryFactory>();
            var mockEmployeeRepo = new Mock<IEmployeeRepository>();

            mockRepoFactory.Setup(m => m.CreateEmployeeRepository()).Returns(mockEmployeeRepo.Object);
            builder.UseSolutionRelativeContentRoot("./");

            builder.ConfigureTestServices(services =>
            {
                //var serviceDescriptor = services.Where(
                //    s => s.ServiceType == typeof(IAuthenticationService) || 
                //         s.ServiceType == typeof(JwtBearerHandler) ||
                //         s.ServiceType == typeof(IAuthenticationHandlerProvider) ||
                //         s.ServiceType == typeof(IAuthenticationSchemeProvider)).ToList();
                //serviceDescriptor.ForEach(d => services.Remove(d));
                
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthenticationExtensions.TEST_SCHEME;
                    options.DefaultChallengeScheme = TestAuthenticationExtensions.TEST_SCHEME;
                }).AddTestAuth(opt => {});
                
                services.AddTransient(provider => mockRepoFactory.Object);
            });
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(new string[]{})
                .UseStartup<TestStartup>();
        }
    }
}