using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TrackerService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IHostingEnvironment hostEnv;

        public ValuesController(IConfiguration config, IHostingEnvironment hostEnv)
        {
            this.config = config;
            this.hostEnv = hostEnv;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
        }

        [Route("time")]
        public ActionResult<string> GetCurrentTime()
        {
            return DateTime.Now.ToLongDateString();
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return $"You sent: {id}";
        }

        [Route("error")]
        public IActionResult Error()
        {
            throw new ApplicationException("This is a test.");
        }

        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("Simple bad request response");
        }

        [HttpGet("config")]
        public IActionResult ShowEnvironmentVariables()
        {
            return Ok(new
            {
                SimpleTaxDB = config.GetConnectionString("SimpleTaxDB"),
                CloudStorage = config.GetConnectionString("CloudStorage"),
                RedisCache = config.GetConnectionString("RedisCache"),
                Environment = hostEnv.EnvironmentName
            });
        }
    }
}
