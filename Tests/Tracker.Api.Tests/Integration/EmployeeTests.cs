using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tracker.Api.Tests.Integration.Application;
using TrackerService.Api.ViewModels;
using TrackerService.Common;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;
using Xunit;

namespace Tracker.Api.Tests.Integration
{
    public class EmployeeTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory factory;
        
        public EmployeeTests(InMemoryWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task GetEmployeeList_ReturnsOK()
        {
            var mockEmployeeRepo = new Mock<IEmployeeRepository>().Object;
            var mockEmpRepoFactory = new Mock<IRepositoryFactory>();
            mockEmpRepoFactory.Setup(m => m.CreateEmployeeRepository()).Returns(mockEmployeeRepo);
            var client = factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services => { services.AddTransient(p => mockEmpRepoFactory.Object); });
                }).CreateClient();

            var response = await client.GetAsync("api/employee");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestGetWhenNotFound_NotFoundResponseIsReturned()
        {
            var mockEmployeeRepo = SetupGetEmployeeById(null);
            var mockEmpRepoFactory = SetupRepositoryFactory(mockEmployeeRepo.Object);

            var http = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddTransient(p => mockEmpRepoFactory.Object); });
            }).CreateClient();

            var response = await http.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/api/employee/22"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task TestGetByIdWhenEmployeeFound_FieldsAreMappedCorrectly()
        {
            var employee = new Employee
            {
                EmployeeId = 1,
                FirstName = "Bob",
                LastName = "Smith",
                DateOfBirth = new DateTime(1955, 1, 22),
                StartDate = new DateTime(2000, 2, 14)
            };
            var mockEmpRepo = SetupGetEmployeeById(employee);
            var mockEmpRepoFactory = SetupRepositoryFactory(mockEmpRepo.Object);
            var http = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddTransient(p => mockEmpRepoFactory.Object); });
            }).CreateClient();

            var response = await http.GetAsync("/api/employee/123");
            var empViewModel = await response.GetContent<EmployeeViewModel>();
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(employee.EmployeeId, empViewModel.EmployeeId);
            Assert.Equal(employee.FirstName, empViewModel.FirstName);
            Assert.Equal(employee.LastName, empViewModel.LastName);
            Assert.Equal(employee.DateOfBirth, empViewModel.DateOfBirth);
            Assert.Equal(employee.StartDate, empViewModel.StartDate);
        }

        private Mock<IEmployeeRepository> SetupGetEmployeeById(Employee employee)
        {
            var mockEmployeeRepo = new Mock<IEmployeeRepository>();
            mockEmployeeRepo.Setup(m => m.GetEmployee(It.IsAny<int>())).ReturnsAsync(employee);
            return mockEmployeeRepo;
        }

        private Mock<IRepositoryFactory> SetupRepositoryFactory(IEmployeeRepository empRepo)
        {
            var mockRepoFactory = new Mock<IRepositoryFactory>();
            mockRepoFactory.Setup(m => m.CreateEmployeeRepository()).Returns(empRepo);
            return mockRepoFactory;
        }
    }
}
