using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return Ok(timesheets.Select(MapToTimesheetViewModel));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Timesheet))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTimesheetAsync(int id)
        {
            var timesheet = await timesheetRepo.GetTimesheet(id);
            if (timesheet != null)
            {
                return Ok(MapToTimesheetViewModel(timesheet));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]TimesheetViewModel vmTimesheet)
        {
            var timesheet = MapToTimesheet(vmTimesheet);
            timesheet.CreatedBy = userContext.UserId;
            timesheet.ModifiedBy = userContext.UserId;

            var result = await timesheetRepo.CreateTimesheet(timesheet);
            return CreatedAtAction(nameof(GetTimesheetAsync), new {id = result.TimesheetId}, result);
        }

        private Timesheet MapToTimesheet(TimesheetViewModel vmTimesheet)
        {
            return new Timesheet
            {
                TimesheetId = vmTimesheet.TimesheetId,
                EmployeeId = vmTimesheet.EmployeeId.Value,
                WorkDate = vmTimesheet.WorkDate.Value,
                TimesheetEntries = vmTimesheet.Entries.Select(MapToTimesheetEntry).ToList()
            };
        }

        private TimesheetEntry MapToTimesheetEntry(TimesheetEntryViewModel vmTimesheetEntry)
        {
            return new TimesheetEntry
            {
                TimesheetEntryId = vmTimesheetEntry.TimesheetEntryId,
                StartDate = vmTimesheetEntry.StartDate.Value,
                EndDate = vmTimesheetEntry.EndDate ?? DateTime.MinValue,
                TimesheetId = vmTimesheetEntry.TimesheetId.Value,
                Notes = vmTimesheetEntry.Notes
            };
        }

        private TimesheetViewModel MapToTimesheetViewModel(Timesheet timesheet)
        {
            return new TimesheetViewModel
            {
                TimesheetId = timesheet.TimesheetId,
                EmployeeId = timesheet.EmployeeId,
                WorkDate = timesheet.WorkDate,
                Entries = timesheet.TimesheetEntries.Select(MapToTimesheetEntryViewModel)
            };
        }

        private TimesheetEntryViewModel MapToTimesheetEntryViewModel(TimesheetEntry tsEntry)
        {
            return new TimesheetEntryViewModel
            {
                TimesheetEntryId = tsEntry.TimesheetEntryId,
                TimesheetId = tsEntry.TimesheetId,
                StartDate = tsEntry.StartDate,
                EndDate = tsEntry.EndDate,
                Notes = tsEntry.Notes
            };
        }
    }
}
