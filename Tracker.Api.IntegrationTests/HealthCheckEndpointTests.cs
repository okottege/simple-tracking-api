using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tracker.Api.IntegrationTests
{
    [TestClass]
    public class HealthCheckEndpointTests : BaseIntegrationTest
    {
        [ClassInitialize]
        public static void InitialiseTests(TestContext context)
        {
            Initialise(context);
        }

        [TestMethod]
        public void TestHealthCheck()
        {
            var token = GetAccessToken().Result;
        }
    }
}
