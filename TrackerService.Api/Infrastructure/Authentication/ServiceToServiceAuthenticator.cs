using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrackerService.Api.Infrastructure.Authentication.Models;
using TrackerService.Api.Infrastructure.Contracts;
using TrackerService.Common;

namespace TrackerService.Api.Infrastructure.Authentication
{
    public class ServiceToServiceAuthenticator : IServiceAuthenticator
    {
        private readonly HttpClient client;
        private readonly AuthenticationConfig config;

        public ServiceToServiceAuthenticator(IHttpClientFactory factory, AuthenticationConfig config)
        {
            client = factory.CreateClient(HttpClientNames.AUTHENTICATION_CLIENT);
            this.config = config;
        }

        public async Task<string> AuthenticateAsync()
        {
            dynamic reqContent = new ExpandoObject();
            reqContent.grant_type = config.GrantType;
            reqContent.client_id = config.ClientID;
            reqContent.client_secret = config.ClientSecret;
            reqContent.audience = config.Audience;

            var content = new StringContent(JsonConvert.SerializeObject(reqContent), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("oauth/token", content);
            var responseBody = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return responseBody["access_token"].ToString();
        }
    }
}
