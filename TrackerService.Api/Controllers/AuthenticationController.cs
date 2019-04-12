using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Api.ViewModels;
using TrackerService.Common.Contracts;
using TrackerService.Data.Contracts;

namespace TrackerService.Api.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAntiforgery antiForgery;
        private readonly IUserAuthenticator authenticator;

        public AuthenticationController(IAntiforgery antiForgery, IUserAuthenticator authenticator)
        {
            this.antiForgery = antiForgery;
            this.authenticator = authenticator;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<ActionResult<IUserAuthenticationResult>> GetToken([FromBody] LoginInformation login)
        {
            var authResult = await authenticator.Authenticate(login.Username, login.Password);
            return Ok(authResult);
        }

        [HttpGet("anti-forgery")]
        public IActionResult GetAntiForgeryToken()
        {
            var tokenSet = antiForgery.GetTokens(HttpContext);
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
