using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrackerService.Api.ViewModels;
using TrackerService.Common;

namespace TrackerService.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly HttpClient client;
        private readonly IConfiguration config;

        public AuthenticationController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            this.config = config;
            client = clientFactory.CreateClient(HttpClientNames.AUTHENTICATION_CLIENT);
        }

        [Route("token")]
        [HttpPost]
        public async Task<ActionResult<object>> GetToken([FromBody]LoginInformation login)
        {
            dynamic reqContent = new ExpandoObject();
            reqContent.grant_type = config["Authentication:GrantType"];
            reqContent.username = login.Username;
            reqContent.password = login.Password;
            reqContent.client_id = config["Authentication:ClientID"];
            reqContent.client_secret = config["Authentication:ClientSecret"];
            reqContent.audience = config["Authentication:Audience"];
            reqContent.realm = config["Authentication:Realm"];

            var content = new StringContent(JsonConvert.SerializeObject(reqContent), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("oauth/token", content);
            var responseBody = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return responseBody;
        }
    }
}
