using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly AuthenticationConfig authConfig;
        private readonly IAntiforgery antiforgery;

        public AuthenticationController(IHttpClientFactory clientFactory, IAntiforgery antiforgery, AuthenticationConfig authConfig)
        {
            this.antiforgery = antiforgery;
            this.authConfig = authConfig;
            client = clientFactory.CreateClient(HttpClientNames.AUTHENTICATION_CLIENT);
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<ActionResult<object>> GetToken([FromBody]LoginInformation login)
        {
            var reqContent = new
            {
                grant_type = authConfig.GrantType,
                username = login.Username,
                password = login.Password,
                client_id = authConfig.ClientID,
                client_secret = authConfig.ClientSecret,
                audience = authConfig.Audience,
                realm = authConfig.Realm
            };
            var content = new StringContent(JsonConvert.SerializeObject(reqContent), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("oauth/token", content);
            var responseBody = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            return responseBody;
        }

        [HttpGet("anti-forgery")]
        public IActionResult GetAntiForgeryToken()
        {
            var tokenSet = antiforgery.GetTokens(HttpContext);
            return Ok(new {requestToken = tokenSet.RequestToken, header = tokenSet.HeaderName, cookieToken = tokenSet.CookieToken});
        }

        [HttpPost("change-password")]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword([FromBody]LoginInformation login)
        {
            return Ok("All good");
        }
    }
}
