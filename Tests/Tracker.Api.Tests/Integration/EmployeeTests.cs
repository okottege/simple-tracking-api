using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Tracker.Api.Tests.Integration.Application;
using Tracker.Api.Tests.Integration.Application.Authentication;
using Xunit;

namespace Tracker.Api.Tests.Integration
{
    public class EmployeeTests : IClassFixture<InMemoryWebApplicationFactory<TestStartup>>
    {
        private readonly HttpClient client;

        public EmployeeTests(InMemoryWebApplicationFactory<TestStartup> factory)
        {
            client = factory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect = false});
        }

        [Fact]
        public async Task GetEmployeeList_ReturnsOK()
        {
            var response = await client.GetAsync("api/employee");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
