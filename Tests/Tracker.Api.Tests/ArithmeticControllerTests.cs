using TrackerService.Api.Controllers;
using Xunit;

namespace Tracker.Api.Tests
{
    public class ArithmeticControllerTests
    {
        private readonly ArithmeticController controller = new ArithmeticController();

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(0, 0, 0)]
        [InlineData(5, -1, 4)]
        [InlineData(-5, -1, -6)]
        public void ShouldPerformBasicAddition(decimal a, decimal b, decimal expectedResult)
        {
            var result = controller.Add(a, b).Value;
            Assert.Equal(expectedResult, result);
        }
    }
}