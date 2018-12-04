using System.Collections.Generic;
using System.Threading.Tasks;
using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Contracts
{
    public interface ITimesheetRepository
    {
        Task<IEnumerable<Timesheet>> GetTimesheets();
        Task<IEnumerable<Timesheet>> GetTimesheets(int employeeId);
        Task<Timesheet> GetTimesheet(int id);
        Task<TimesheetEntry> GetTimesheetEntry(int id);
        Task<IEnumerable<TimesheetEntry>> GetTimesheetEntries(int timesheetId);

        Task<Timesheet> CreateTimesheet(Timesheet timesheet);
        Task<TimesheetEntry> CreateTimesheetEntry(TimesheetEntry timesheetEntry);
    }
}