using TrackerService.Api.Controllers;
using Xunit;

namespace Tracker.Api.Tests
{
    public class ValuesControllerTests
    {
        [Fact]
        public void ShouldReturnAListOfValues()
        {
            var controller = new ValuesController();
            var getResult = controller.Get();
            var resultBody = getResult.Value;

            Assert.NotNull(resultBody);
            Assert.NotEmpty(resultBody);
        }
    }
}
