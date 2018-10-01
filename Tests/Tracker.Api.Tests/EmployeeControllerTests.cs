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
        public async Task TestGetWhenNotFound_NotFoundResponseIsReturned()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            mockRepo.Setup(m => m.GetEmployee(It.IsAny<int>())).ReturnsAsync((Employee) null); 
            factory.Setup(m => m.CreateEmployeeRepository()).Returns(mockRepo.Object);
            var controller = new EmployeeController(factory.Object);

            var response = await controller.GetEmployee(100);
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public async Task TestGetByIdWhenEmployeeFound_FieldsAreMappedCorrectly()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var employee = new Employee
            {
                EmployeeId = 1,
                FirstName = "Bob",
                LastName = "Smith",
                DateOfBirth = new DateTime(1955, 1, 22),
                StartDate = new DateTime(2000, 2, 14)
            };
            mockRepo.Setup(m => m.GetEmployee(It.IsAny<int>())).ReturnsAsync(employee);
            factory.Setup(m => m.CreateEmployeeRepository()).Returns(mockRepo.Object);
            var controller = new EmployeeController(factory.Object);

            var response = await controller.GetEmployee(100);

            var responsePayload = Assert.IsType<OkObjectResult>(response).Value as EmployeeViewModel;
            Assert.Equal(employee.EmployeeId, responsePayload.EmployeeId);
            Assert.Equal(employee.FirstName, responsePayload.FirstName);
            Assert.Equal(employee.LastName, responsePayload.LastName);
            Assert.Equal(employee.DateOfBirth, responsePayload.DateOfBirth);
            Assert.Equal(employee.StartDate, responsePayload.StartDate);
        }

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
