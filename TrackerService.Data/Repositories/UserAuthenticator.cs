using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrackerService.Common;
using TrackerService.Common.Contracts;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects.UserManagement;
using TrackerService.Data.ServiceAccessResponses;

namespace TrackerService.Data.Repositories
{
    public class UserAuthenticator : IUserAuthenticator
    {
        private readonly HttpClient http;
        private readonly AuthenticationConfig authConfig;

        public UserAuthenticator(IHttpClientFactory httpFactory, AuthenticationConfig authConfig)
        {
            this.authConfig = authConfig;
            http = httpFactory.CreateClient(HttpClientNames.AUTHENTICATION_CLIENT);
        }

        public async Task<IUserAuthenticationResult> Authenticate(string username, string password)
        {
            var reqContent = new
            {
                grant_type = authConfig.GrantType,
                username,
                password,
                client_id = authConfig.ClientID,
                client_secret = authConfig.ClientSecret,
                audience = authConfig.Audience,
                realm = authConfig.Realm
            };
            var content = new StringContent(JsonConvert.SerializeObject(reqContent), Encoding.UTF8, "application/json");
            var response = await http.PostAsync("oauth/token", content);
            var responseBody = JsonConvert.DeserializeObject<UserAuthenticationResponse>(await response.Content.ReadAsStringAsync());
            return new UserAuthenticationResult { AccessToken = responseBody.access_token, ExpiresIn = responseBody.expires_in, Scope = responseBody.scope};
        }
    }
}
