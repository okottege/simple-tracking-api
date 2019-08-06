using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tracker.Api.Tests.Integration.Application;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;
using TrackerService.Core.Repositories;
using TrackerService.Data.Contracts;
using Xunit;

namespace Tracker.Api.Tests.Integration
{
    public class TaskControllerShould : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory factory;
        private const string SampleCreateTaskUrl = "/api/tasks";

        public TaskControllerShould(InMemoryWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task ReturnErrorWhenNoRequestBodyProvided()
        {
            var mockTaskRepo = Substitute.For<ITaskRepository>();
            var http = CreateHttpClient(mockTaskRepo);

            var response = await http.PostAsync(SampleCreateTaskUrl, new StringContent(string.Empty, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsBadRequestResponse_WhenTitleIsNotProvided()
        {
            var createTaskPayload = new CreateTaskViewModel().ToJsonContent();
            var http = CreateHttpClient(Substitute.For<ITaskRepository>());

            var response = await http.PostAsync(SampleCreateTaskUrl, createTaskPayload);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("The Title field is required", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ReturnsPayloadWithTaskId_WhenTaskIsCreated()
        {
            var createTaskPayload = new CreateTaskViewModel{Title = "Test task"}.ToJsonContent();
            var mockTaskRepo = Substitute.For<ITaskRepository>();
            var newTaskId = Guid.NewGuid().ToString();
            mockTaskRepo.CreateNewTask(Arg.Any<ITask>()).Returns(newTaskId);
            var http = CreateHttpClient(mockTaskRepo);

            var response = await http.PostAsync(SampleCreateTaskUrl, createTaskPayload);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains($@"""taskId"":""{newTaskId}""", await response.Content.ReadAsStringAsync());
        }

        private HttpClient CreateHttpClient(ITaskRepository mockTaskRepo)
        {
            var mockRepoFactory = Substitute.For<IRepositoryFactory>();
            mockRepoFactory.CreateTaskRepository().Returns(mockTaskRepo);
            return factory.WithWebHostBuilder(
                        b => b.ConfigureTestServices(services => services.AddTransient(p => mockRepoFactory))
                    ).CreateClient();
        }
    }
}
