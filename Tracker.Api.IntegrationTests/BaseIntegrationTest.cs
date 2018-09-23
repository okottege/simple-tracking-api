using System;
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
        protected static HttpClient serviceClient;
        protected static HttpClient authClient;
        protected static TestConfiguration config;

        public static void Initialise(TestContext testContext)
        {
            config = new TestConfiguration(testContext);
            var clientFactory = new HttpClientFactory(config.WebApiUrl, config.Authentication.AuthenticationBaseUrl);

            serviceClient = clientFactory.CreateNewServiceClient();
            authClient = clientFactory.CreateNewAuthenticationClient();
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
            var response = await authClient.PostAsync(String.Empty, new FormUrlEncodedContent(payload));
            var strContent = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<JObject>(strContent);

            return content["access_token"].ToString();
        }
    }
}
