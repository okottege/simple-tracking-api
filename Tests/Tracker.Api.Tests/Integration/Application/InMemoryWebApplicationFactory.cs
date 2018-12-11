using AutoMapper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TrackerService.Api.Infrastructure.Contracts;
using TrackerService.Api.Infrastructure.Filters;
using TrackerService.Data.Contracts;

namespace Tracker.Api.Tests.Integration.Application
{
    public class InMemoryWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class 
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

                services.AddAutoMapper();
                services.AddTransient<RequireServiceToken>();
            });
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(new string[]{})
                .UseStartup<TStartup>();
        }
    }
}
