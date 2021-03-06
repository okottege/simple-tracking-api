﻿using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrackerService.Common;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient http;
        private readonly UserManagementConfig config;

        public UserRepository(IHttpClientFactory httpFactory, UserManagementConfig config)
        {
            http = httpFactory.CreateClient(HttpClientNames.USER_MANAGEMENT_CLIENT);
            this.config = config;
        }

        public async Task<User> Register(UserRegistration registration)
        {
            dynamic reqContent = new ExpandoObject();
            reqContent.connection = config.ConnectionID;
            reqContent.email = registration.Email;
            reqContent.name = registration.Email;
            reqContent.password = registration.Password;

            var content = new StringContent(JsonConvert.SerializeObject(reqContent), Encoding.UTF8, "application/json");
            var response = await http.PostAsync("users", content);

            var responseBody = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return new User
            {
                Id =  responseBody.SelectToken("identities[0].user_id").ToString(),
                Email = responseBody["email"].ToString()
            };
        }

        public Task<User> GetUser(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
