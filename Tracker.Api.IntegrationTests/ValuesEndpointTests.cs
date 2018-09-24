using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Tracker.Api.IntegrationTests
{
    [TestClass]
    public class ValuesEndpointTests : BaseIntegrationTest
    {
        [ClassInitialize]
        public static async Task InitialiseTests(TestContext testContext)
        {
            await Initialise(testContext);
        }

        [TestMethod]
        public async Task TestGet()
        {
            var result = await serviceClient.GetAsync("api/values");
            var values = JsonConvert.DeserializeObject<string[]>(await result.Content.ReadAsStringAsync());
            Assert.IsTrue(values.Length == 2);
        }
    }
}
