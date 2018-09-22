using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracker.Api.IntegrationTests.Factories;

namespace Tracker.Api.IntegrationTests
{
    public class BaseIntegrationTest
    {
        private static readonly HttpClientFactory clientFactory = new HttpClientFactory();
        protected static HttpClient client;

        [ClassInitialize]
        public static void InitialiseTests(TestContext testContext)
        {
            client = clientFactory.CreateNewClient();
        }
    }
}
