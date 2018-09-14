using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace TrackerService.Api.Controllers
{
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        public ActionResult<object> Get()
        {
            return new
            {
                Version = Assembly.GetEntryAssembly()
                                  .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                  .InformationalVersion
            };
        }
    }
}
