using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Tracker.Api.IntegrationTests
{
    [TestClass]
    public class ValuesEndpointTests
    {
        private static TestContext context;
        private static readonly HttpClientHandler httpClientHandler = new HttpClientHandler();
        
        private static HttpClient client;

        [ClassInitialize]
        public static void InitialiseTests(TestContext testContext)
        {
            context = testContext;
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            client = new HttpClient(httpClientHandler);
        }

        [TestMethod]
        public void TestGet()
        {
            Console.WriteLine($"The web URL is: {context.Properties["webUrl"]}");
            client.BaseAddress = new Uri(context.Properties["webUrl"].ToString());

            var result = client.GetAsync("api/values").Result;
            var values = JsonConvert.DeserializeObject<string[]>(result.Content.ReadAsStringAsync().Result);
            Assert.IsTrue(values.Length == 2);
        }
    }
}
