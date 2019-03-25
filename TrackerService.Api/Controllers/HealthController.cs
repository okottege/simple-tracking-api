using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Data.Contracts;

namespace TrackerService.Api.Controllers
{
    [Route("api/health")]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepo;

        public HealthController(IRepositoryFactory factory)
        {
            employeeRepo = factory.CreateEmployeeRepository();
        }

        public async Task<ActionResult<object>> Get()
        {
            var employees = await employeeRepo.GetAllEmployees();

            return new
            {
                Version = Assembly.GetEntryAssembly()
                                  .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                  .InformationalVersion,
                NumEmployees = employees.Count()
            };
        }
    }
}
