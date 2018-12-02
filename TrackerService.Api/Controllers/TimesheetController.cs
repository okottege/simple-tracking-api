using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerService.Api.Infrastructure.Contracts;
using TrackerService.Api.ViewModels;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/timesheet")]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetRepository timesheetRepo;
        private readonly IUserContext userContext;
        private readonly IMapper mapper;

        public TimesheetController(IRepositoryFactory factory, IUserContext userContext, IMapper mapper)
        {
            timesheetRepo = factory.CreateTimesheetRepository();
            this.userContext = userContext;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Timesheet>))]
        public async Task<IActionResult> GetAllTimesheets()
        {
            var timesheets = await timesheetRepo.GetTimesheets();
            return Ok(timesheets.Select(t => mapper.Map<TimesheetViewModel>(t)));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Timesheet))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTimesheetAsync(int id)
        {
            var timesheet = await timesheetRepo.GetTimesheet(id);
            if (timesheet != null)
            {
                return Ok(mapper.Map<TimesheetViewModel>(timesheet));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]TimesheetViewModel vmTimesheet)
        {
            var timesheet = mapper.Map<Timesheet>(vmTimesheet);
            timesheet.CreatedBy = userContext.UserId;
            timesheet.ModifiedBy = userContext.UserId;

            var result = await timesheetRepo.CreateTimesheet(timesheet);
            return CreatedAtAction(nameof(GetTimesheetAsync), new {id = result.TimesheetId}, result);
        }
    }
}
