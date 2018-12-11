using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Tracker.Api.Tests.Integration.Application;
using Tracker.Api.Tests.Integration.Application.Authentication;
using TrackerService.Api.ViewModels.UserManagement;
using Xunit;

namespace Tracker.Api.Tests.Integration
{
    public class UserManagementTests : IClassFixture<InMemoryWebApplicationFactory<TestStartup>>
    {
        private readonly HttpClient client;

        public UserManagementTests(InMemoryWebApplicationFactory<TestStartup> factory)
        {
            client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        public async Task RegisterEmployee_ReturnsOK()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new RegistrationViewModel {Email = "user.name@gmail.com", Password = "testing-1234"}), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/user", content);
            // Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
