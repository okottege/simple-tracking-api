using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
