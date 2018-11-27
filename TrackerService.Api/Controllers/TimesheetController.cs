using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Api.Infrastructure.Contracts;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/timesheets")]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetRepository timesheetRepo;
        private readonly IUserContext userContext;

        public TimesheetController(IRepositoryFactory factory, IUserContext userContext)
        {
            timesheetRepo = factory.CreateTimesheetRepository();
            this.userContext = userContext;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Timesheet>))]
        public async Task<IActionResult> GetAllTimesheets()
        {
            var timesheets = await timesheetRepo.GetTimesheets();
            return Ok(timesheets);
        }
    }
}
