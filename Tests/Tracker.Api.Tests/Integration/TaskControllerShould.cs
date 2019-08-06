using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tracker.Api.Tests.Integration.Application;
using TrackerService.Core.Repositories;
using TrackerService.Data.Contracts;
using Xunit;

namespace Tracker.Api.Tests.Integration
{
    public class TaskControllerShould : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory factory;

        public TaskControllerShould(InMemoryWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task ReturnErrorWhenNoRequestBodyProvided()
        {
            var mockTaskRepo = Substitute.For<ITaskRepository>();
            var mockRepoFactory = Substitute.For<IRepositoryFactory>();
            mockRepoFactory.CreateTaskRepository().Returns(mockTaskRepo);
            var http = factory.WithWebHostBuilder(b => b.ConfigureTestServices(services => services.AddTransient(p => mockRepoFactory)))
                .CreateClient();

            var response = await http.PostAsync("/api/tasks", new StringContent(string.Empty, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
