using Microsoft.Extensions.Configuration;
using NSubstitute;
using TrackerService.Api.Controllers;
using Xunit;

namespace Tracker.Api.Tests
{
    public class ValuesControllerTests
    {
        private readonly ValuesController controller = new ValuesController(Substitute.For<IConfiguration>());

        [Fact]
        public void ShouldReturnAListOfValues()
        {
            var getResult = controller.Get();
            var resultBody = getResult.Value;

            Assert.NotNull(resultBody);
            Assert.NotEmpty(resultBody);
        }

        [Fact]
        public void ShouldReturnNonEmptyValueForCurrentDate()
        {
            var timeResult = controller.GetCurrentTime().Value;
            Assert.NotNull(timeResult);
            Assert.True(timeResult.Length > 0);
        }
    }
}
