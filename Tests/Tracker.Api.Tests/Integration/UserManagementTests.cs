using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Tracker.Api.Tests.Integration.Application;
using Tracker.Api.Tests.Integration.Application.Authentication;
using TrackerService.Api.Infrastructure.Contracts;
using TrackerService.Api.ViewModels.UserManagement;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;
using Xunit;

namespace Tracker.Api.Tests.Integration
{
    public class UserManagementTests : IClassFixture<InMemoryWebApplicationFactory<TestStartup>>
    {
        private readonly InMemoryWebApplicationFactory<TestStartup> factory;
        
        public UserManagementTests(InMemoryWebApplicationFactory<TestStartup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task WhenRegisteringNewUser_ServiceTokenShouldBeProvidedToUserRepository()
        {
            UserRegistration regArg = null;

            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(m => m.Register(It.IsAny<UserRegistration>()))
                .ReturnsAsync(new User())
                .Callback<UserRegistration>(registration => regArg = registration);

            var mockAuthenticator = new Mock<IServiceAuthenticator>();
            mockAuthenticator.Setup(m => m.AuthenticateAsync()).ReturnsAsync("service-token-123");

            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient(provider => mockUserRepo.Object);
                    services.AddTransient(provider => mockAuthenticator.Object);
                });
            }).CreateClient();
            var content = GetSampleRegistrationViewModel();

            await client.PostAsync("api/user", content);

            Assert.Equal("service-token-123", regArg.ServiceToken);
        }

        [Fact]
        public async Task RegisterEmployee_ReturnsOK()
        {
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(m => m.Register(It.IsAny<UserRegistration>()))
                .ReturnsAsync(new User { Email = "user.name@gmail.com", Id = "usr-001" });
            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient(provider => mockUserRepo.Object);
                    services.AddTransient(provider => new Mock<IServiceAuthenticator>().Object);
                });
            }).CreateClient();
            var content = GetSampleRegistrationViewModel();

            var response = await client.PostAsync("api/user", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory(DisplayName = "User registration without mandatory data")]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("bob.smith@hotmail.com", null)]
        [InlineData(null, "password01")]
        public async Task RegisterEmployeeWithMissingData_ReturnsBadRequest(string email, string password)
        {
            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient(provider => new Mock<IServiceAuthenticator>().Object);
                    services.AddTransient(provider => new Mock<IUserRepository>().Object);
                });
            }).CreateClient();

            var response = await client.PostAsync("api/user", GetSampleRegistrationViewModel(email, password));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private StringContent GetSampleRegistrationViewModel(string email = "user.name@gmail.com", string password = "testing-1234")
        {
            return new StringContent(
                JsonConvert.SerializeObject(new RegistrationViewModel { Email = email, Password = password }),
                Encoding.UTF8,
                "application/json");
        }
    }
}
