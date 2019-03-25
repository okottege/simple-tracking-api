using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TrackerService.Api.Controllers;
using TrackerService.Api.ViewModels;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;
using Xunit;

namespace Tracker.Api.Tests
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IRepositoryFactory> factory = new Mock<IRepositoryFactory>();

        [Fact]
        public async Task TestCreateEmployee_ReturnsCreatedAtResult()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var vmEmployee = new Employee {EmployeeId = 100};
            mockRepo.Setup(m => m.Create(It.IsAny<Employee>())).ReturnsAsync(vmEmployee);
            factory.Setup(m => m.CreateEmployeeRepository()).Returns(mockRepo.Object);
            var controller = new EmployeeController(factory.Object);

            var response = await controller.CreateAsync(new EmployeeViewModel {DateOfBirth = new DateTime(), StartDate = new DateTime()});

            var result = Assert.IsType<CreatedAtActionResult>(response);
            Assert.Equal("GetEmployee", result.ActionName);
            Assert.Equal(100, result.RouteValues["employeeId"]);
        }
    }
}
