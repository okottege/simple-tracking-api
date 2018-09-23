using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Tracker.Api.IntegrationTests
{
    [TestClass]
    public class ValuesEndpointTests : BaseIntegrationTest
    {
        [ClassInitialize]
        public static void InitialiseTests(TestContext testContext)
        {
            Initialise(testContext);
        }

        [TestMethod]
        public void TestGet()
        {
            var result = serviceClient.GetAsync("api/values").Result;
            var values = JsonConvert.DeserializeObject<string[]>(result.Content.ReadAsStringAsync().Result);
            Assert.IsTrue(values.Length == 2);
        }
    }
}
