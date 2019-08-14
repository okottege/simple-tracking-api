using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tracker.Api.Tests.CoreDomain;
using Tracker.Api.Tests.Integration.Application;
using TrackerService.Api.ViewModels.Tasks;
using TrackerService.Common;
using TrackerService.Core.CoreDomain.Tasks.Definitions;
using TrackerService.Core.Repositories;
using TrackerService.Core.Tasks.TaskCreation;
using Xunit;

namespace Tracker.Api.Tests.Integration
{
    public class TaskControllerShould : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory factory;
        private const string BaseTaskUrl = "/api/tasks";

        public TaskControllerShould(InMemoryWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task ReturnErrorWhenNoRequestBodyProvided()
        {
            var mockTaskRepo = Substitute.For<ITaskRepository>();
            var mockTaskCreator = Substitute.For<ITaskCreator>();
            var http = CreateHttpClient(mockTaskRepo, mockTaskCreator);

            var response = await http.PostAsync(BaseTaskUrl, new StringContent(string.Empty, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsBadRequestResponse_WhenTitleIsNotProvided()
        {
            var createTaskPayload = new CreateTaskViewModel().ToJsonContent();
            var http = CreateHttpClient(Substitute.For<ITaskRepository>(), Substitute.For<ITaskCreator>());

            var response = await http.PostAsync(BaseTaskUrl, createTaskPayload);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("The Title field is required", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ReturnsPayloadWithTaskId_WhenTaskIsCreated()
        {
            var createTaskPayload = new CreateTaskViewModel{Title = "Test task"}.ToJsonContent();
            var mockTaskCreator = Substitute.For<ITaskCreator>();
            var newTaskId = Guid.NewGuid().ToString();
            mockTaskCreator.CreateTask(Arg.Any<ITask>()).Returns(newTaskId);
            var http = CreateHttpClient(Substitute.For<ITaskRepository>(), mockTaskCreator);

            var response = await http.PostAsync(BaseTaskUrl, createTaskPayload);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains($"{BaseTaskUrl}/{newTaskId}", response.Headers.Location.ToString());
            Assert.Contains($@"""taskId"":""{newTaskId}""", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ReturnsTaskDetails_ForTaskDetails()
        {
            var mockTaskRepo = Substitute.For<ITaskRepository>();
            var mockTaskCreator = Substitute.For<ITaskCreator>();
            var testTask = new TestTask()
                .WithBasicData("task-1234")
                .WithNumberOfAssignments(2)
                .WithNumberOfContextItems(2);
            mockTaskRepo.GetTask(Arg.Any<string>()).Returns(testTask);
            var http = CreateHttpClient(mockTaskRepo, mockTaskCreator);

            var response = await http.GetAsync($"{BaseTaskUrl}/task-1234");

            var vmContent = await response.GetContent<TaskDetailsViewModel>();
            Assert.Equal(testTask.Title, vmContent.Title);
            Assert.Equal(testTask.Description, vmContent.Description);
            Assert.Equal(testTask.DueDate, vmContent.DueDate);
        }

        private HttpClient CreateHttpClient(ITaskRepository mockTaskRepo, ITaskCreator mockTaskCreator)
        {
            return factory.WithWebHostBuilder(
                        b => b.ConfigureTestServices(services =>
                        {
                            services.AddTransient(p => mockTaskRepo);
                            services.AddTransient(s => mockTaskCreator);
                        })
                    ).CreateClient();
        }
    }
}
