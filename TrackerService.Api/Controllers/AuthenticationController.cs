using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Api.ViewModels;

namespace TrackerService.Api.Controllers
{
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly HttpClient client;

        public AuthenticationController(HttpClient client)
        {
            this.client = client;
        }

        [Route("token")]
        [HttpPost]
        public Task<IActionResult> GetToken([FromBody]LoginInformation login)
        {
            throw new ApplicationException();
        }
    }
}
