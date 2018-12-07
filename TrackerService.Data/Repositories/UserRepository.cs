using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrackerService.Common;
using TrackerService.Common.Contracts;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient http;
        private readonly IUserRepositoryConfig config;

        public UserRepository(IHttpClientFactory httpFactory, IUserRepositoryConfig config)
        {
            http = httpFactory.CreateClient(HttpClientNames.USER_MANAGEMENT_CLIENT);
            this.config = config;
        }

        public async Task<User> Register(UserRegistration registration)
        {
            dynamic reqContent = new ExpandoObject();
            reqContent.connection = config.ConnectionName;
            reqContent.email = registration.Email;
            reqContent.password = registration.Password;

            var content = new StringContent(JsonConvert.SerializeObject(reqContent), Encoding.UTF8, "application/json");
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {registration.ServiceToken}");

            var response = await http.PostAsync("users", content);
            response.EnsureSuccessStatusCode();

            var responseBody = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return new User
            {
                Id =  responseBody["identities.user_id"].ToString(),
                Email = responseBody["email"].ToString()
            };
        }

        public Task<User> GetUser(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
