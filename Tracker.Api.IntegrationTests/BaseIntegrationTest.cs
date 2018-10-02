using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tracker.Api.IntegrationTests.Configuration;
using Tracker.Api.IntegrationTests.Factories;
using Tracker.Api.IntegrationTests.Logging;

namespace Tracker.Api.IntegrationTests
{
    public class BaseIntegrationTest
    {
        protected static HttpClient serviceClient;
        protected static TestConfiguration config;
        protected static ITestTraceLogger logger;

        public static async Task Initialise(TestContext testContext)
        {
            config = new TestConfiguration(testContext);
            logger = new TestTraceTraceLogger(testContext);
            var clientFactory = new HttpClientFactory(config.WebApiUrl, config.Authentication.AuthenticationBaseUrl);
            var authClient = clientFactory.CreateNewAuthenticationClient();

            serviceClient = clientFactory.CreateNewServiceClient();
            await InitialiseServiceClient(authClient);
        }

        private static async Task InitialiseServiceClient(HttpClient authClient)
        {
            var accessToken = await GetAccessToken(authClient);
            serviceClient.DefaultRequestHeaders.Clear();
            serviceClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        }

        private static async Task<string> GetAccessToken(HttpClient authClient)
        {
            var payload = new Dictionary<string, string>();
            payload["client_id"] = config.Authentication.ClientId;
            payload["audience"] = config.Authentication.Audience;
            payload["grant_type"] = "client_credentials";
            payload["client_secret"] = config.Authentication.ClientSecret;

            var response = await authClient.PostAsync(string.Empty, new FormUrlEncodedContent(payload));
            var strContent = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<JObject>(strContent);

            return content["access_token"].ToString();
        }

        protected static async Task<dynamic> GetResponsePayload(HttpResponseMessage response)
        {
            var respContentStr = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(respContentStr);
        }
    }
}
