using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tracker.Api.IntegrationTests
{
    [TestClass]
    public class ValuesEndpointTests
    {
        private static TestContext context;

        [ClassInitialize]
        public static void InitialiseTests(TestContext testContext)
        {
            context = testContext;
        }

        [TestMethod]
        public void TestGet()
        {
            Console.WriteLine($"The web URL is: {context.Properties["webUrl"]}");
            Assert.IsTrue(true);
        }
    }
}
