using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Api.Controllers
{
    [ApiController]
    [Route("api/timesheets")]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetRepository timesheetRepo;

        public TimesheetController(IRepositoryFactory factory)
        {
            timesheetRepo = factory.CreateTimesheetRepository();
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
