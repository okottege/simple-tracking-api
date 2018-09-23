using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tracker.Api.IntegrationTests.Configuration;
using Tracker.Api.IntegrationTests.Factories;

namespace Tracker.Api.IntegrationTests
{
    public class BaseIntegrationTest
    {
        private static readonly HttpClientFactory clientFactory = new HttpClientFactory();
        protected static HttpClient client;
        protected static TestConfiguration config;

        public static void Initialise(TestContext testContext)
        {
            client = clientFactory.CreateNewClient();
            config = new TestConfiguration(testContext);
        }

        protected static async Task<string> GetAccessToken()
        {
            var payload = new Dictionary<string, string>();
            payload["client_id"] = config.Authentication.ClientId;
            payload["resource"] = config.Authentication.Resource;
            payload["grant_type"] = "password";
            payload["username"] = config.Authentication.Username;
            payload["password"] = config.Authentication.Password;
            payload["client_secret"] = config.Authentication.ClientSecret;
            var response = await client.PostAsync(config.Authentication.AuthenticationBaseUrl, new FormUrlEncodedContent(payload));
            var strContent = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<JObject>(strContent);

            return content["access_token"].ToString();
        }
    }
}
