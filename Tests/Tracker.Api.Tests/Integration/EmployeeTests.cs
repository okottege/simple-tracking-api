using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tracker.Api.Tests.Integration.Application;
using TrackerService.Api.ViewModels;
using TrackerService.Common;
using TrackerService.Common.Exceptions;
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
            var mockEmployeeRepo = Substitute.For<IEmployeeRepository>();
            var repoFactory = Substitute.For<IRepositoryFactory>();
            repoFactory.CreateEmployeeRepository().Returns(mockEmployeeRepo);

            var client = factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services => { services.AddTransient(p => repoFactory); });
                }).CreateClient();

            var response = await client.GetAsync("api/employee");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestGetWhenNotFound_NotFoundResponseIsReturned()
        {
            var mockEmployeeRepo = SetupGetEmployeeByIdThrowException(new EntityNotFoundException("task not found"));
            var mockEmpRepoFactory = SetupRepositoryFactory(mockEmployeeRepo);

            var http = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddTransient(p => mockEmpRepoFactory); });
            }).CreateClient();

            var response = await http.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/api/employee/22"));
            var content = await response.GetContent<dynamic>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("task not found", content.message.ToString());
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
            var mockEmpRepoFactory = SetupRepositoryFactory(mockEmpRepo);
            var http = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddTransient(p => mockEmpRepoFactory); });
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

        private IEmployeeRepository SetupGetEmployeeById(Employee employee)
        {
            var mockEmployeeRepo = Substitute.For<IEmployeeRepository>();
            mockEmployeeRepo.GetEmployee(Arg.Any<int>()).Returns(employee);
            return mockEmployeeRepo;
        }

        private IRepositoryFactory SetupRepositoryFactory(IEmployeeRepository empRepo)
        {
            var mockRepoFactory = Substitute.For<IRepositoryFactory>();
            mockRepoFactory.CreateEmployeeRepository().Returns(empRepo);
            return mockRepoFactory;
        }

        private IEmployeeRepository SetupGetEmployeeByIdThrowException(Exception ex)
        {
            var empRepo = Substitute.For<IEmployeeRepository>();
            empRepo.GetEmployee(Arg.Any<int>()).Throws(ex);
            return empRepo;
        }
    }
}
