using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tracker.Api.IntegrationTests
{
    [TestClass]
    public class HealthCheckEndpointTests : BaseIntegrationTest
    {
        [ClassInitialize]
        public static void InitialiseTests(TestContext context)
        {
            Initialise(context);

            var token = GetAccessToken().Result;
            serviceClient.DefaultRequestHeaders.Clear();
            serviceClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        [TestMethod]
        public void TestHealthCheck()
        {
            var respHealthCheck = serviceClient.GetAsync("/api/health").Result;
            var strHealthContent = respHealthCheck.Content.ReadAsStringAsync().Result;
            var content = JsonConvert.DeserializeObject<JObject>(strHealthContent);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(content["version"].ToString()));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(content["numEmployees"].ToString()));
        }
    }
}
