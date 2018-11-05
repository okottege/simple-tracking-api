using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tracker.Api.Tests.Integration.Application.Authentication;
using TrackerService.Data.Contracts;

namespace Tracker.Api.Tests.Integration.Application
{
    public class InMemoryWebApplicationFactory<TStartup> : WebApplicationFactory<TrackerService.Api.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var mockRepoFactory = new Mock<IRepositoryFactory>();
            var mockEmployeeRepo = new Mock<IEmployeeRepository>();

            mockRepoFactory.Setup(m => m.CreateEmployeeRepository()).Returns(mockEmployeeRepo.Object);

            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthenticationExtensions.TEST_SCHEME;
                    options.DefaultChallengeScheme = TestAuthenticationExtensions.TEST_SCHEME;
                }).AddTestAuth(opt => {});

                services.AddTransient(provider => mockRepoFactory.Object);
            });
        }
    }
}
