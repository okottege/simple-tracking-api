using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tracker.Api.Tests.Integration.Application;
using TrackerService.Core.Tasks.TaskRetrieval;
using Xunit;

namespace Tracker.Api.Tests.Integration
{
    public class TaskSearchControllerShould : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly InMemoryWebApplicationFactory factory;

        public TaskSearchControllerShould(InMemoryWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task ReturnTasksSuccessfullyWithOKResponseCode()
        {
            var http = CreateClient(Substitute.For<ITaskRetriever>());

            var response = await http.GetAsync(Constants.BaseTaskUrl);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private HttpClient CreateClient(ITaskRetriever mockTaskRetriever)
        {
            return factory.WithWebHostBuilder(
                builder => builder.ConfigureTestServices(services =>
                {
                    services.AddTransient(p => mockTaskRetriever);
                })).CreateClient();
        }
    }
}
