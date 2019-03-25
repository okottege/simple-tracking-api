﻿using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Tracker.Api.Tests.Integration.Application;
using TrackerService.Api.ViewModels.UserManagement;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;
using Xunit;

namespace Tracker.Api.Tests.Integration
{
    public class UserManagementTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory factory;
        
        public UserManagementTests(InMemoryWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task RegisterEmployee_ReturnsOK()
        {
            var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var mockUserRepo = new Mock<IUserRepository>();
                    mockUserRepo.Setup(m => m.Register(It.IsAny<UserRegistration>()))
                        .ReturnsAsync(new User { Email = "user.name@gmail.com", Id = "usr-001" });
                    services.AddTransient(p => mockUserRepo.Object);
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
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient(provider => new Mock<IUserRepository>().Object);
                    AddRepositoryFactory(services);
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

        private static void AddRepositoryFactory(IServiceCollection services)
        {
            var mockRepoFactory = new Mock<IRepositoryFactory>();
            mockRepoFactory.Setup(m => m.CreateEmployeeRepository()).Returns(new Mock<IEmployeeRepository>().Object);
            services.AddTransient(provider => mockRepoFactory);
        }
    }
}
