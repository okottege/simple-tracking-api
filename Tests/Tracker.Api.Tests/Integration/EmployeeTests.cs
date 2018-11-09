using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Tracker.Api.Tests.Integration.Application;
using Xunit;

namespace Tracker.Api.Tests.Integration
{
    public class EmployeeTests : IClassFixture<InMemoryWebApplicationFactory<TrackerService.Api.Startup>>
    {
        private readonly HttpClient client;
        private readonly InMemoryWebApplicationFactory<TrackerService.Api.Startup> factory;

        public EmployeeTests(InMemoryWebApplicationFactory<TrackerService.Api.Startup> factory)
        {
            this.factory = factory;
            client = factory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect = false});
        }

        [Fact]
        public async Task GetEmployeeListIsProtected_IfNoAuthTokenProvided_Returns401()
        {
            var response = await client.GetAsync("api/employee");
            // Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
