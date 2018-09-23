using System.Net.Http.Headers;
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
        }

        [TestMethod]
        public void TestHealthCheck()
        {
            var token = GetAccessToken().Result;
            serviceClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respHealthCheck = serviceClient.GetAsync("/api/health").Result;
            var strHealthContent = respHealthCheck.Content.ReadAsStringAsync().Result;
            var content = JsonConvert.DeserializeObject<JObject>(strHealthContent);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(content["Version"].ToString()));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(content["NumEmployees"].ToString()));
        }
    }
}
