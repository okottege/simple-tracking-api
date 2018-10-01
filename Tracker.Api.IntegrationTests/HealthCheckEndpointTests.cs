using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tracker.Api.IntegrationTests
{
    [TestClass]
    public class HealthCheckEndpointTests : BaseIntegrationTest
    {
        [ClassInitialize]
        public static async Task InitialiseTests(TestContext context)
        {
            await Initialise(context);
        }

        [TestMethod]
        public async Task TestHealthCheck()
        {
            var respHealthCheck = await serviceClient.GetAsync("/api/health");
            var strHealthContent = await respHealthCheck.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<JObject>(strHealthContent);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(content["Version"].ToString()));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(content["NumEmployees"].ToString()));
        }
    }
}
