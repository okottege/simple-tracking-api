using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TrackerService.Common.Contracts;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Repositories
{
    internal class TimesheetSqlRepository : BaseSqlRepository, ITimesheetRepository
    {
        private readonly IUserContext userContext;

        internal TimesheetSqlRepository(string connString, IUserContext userContext) : base(connString)
        {
            this.userContext = userContext;
        }

        public async Task<IEnumerable<Timesheet>> GetTimesheets()
        {
            const string SQL_QUERY = @"SELECT * FROM Timesheet";
            return await QueryAsync<Timesheet>(SQL_QUERY, null);
        }

        public async Task<IEnumerable<Timesheet>> GetTimesheets(int employeeId)
        {
            const string SQL_QUERY = @"SELECT * FROM Timesheet WHERE employeeId = @employeeId";
            return await QueryAsync<Timesheet>(SQL_QUERY, new {employeeId});
        }

        public async Task<Timesheet> GetTimesheet(int id)
        {
            const string SQL_QUERY = @"
            SELECT distinct t.* from Timesheet INNER JOIN timesheetEntry te on t.timesheetId = te.timesheetId WHERE t.timesheetId = @timesheetId;
            SELECT te.* from Timesheet INNER JOIN timesheetEntry te on t.timesheetId = te.timesheetId WHERE t.timesheetId = @timesheetId;";

            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var result = await conn.QueryMultipleAsync(SQL_QUERY, new { timesheetId = id }))
                {
                    var timesheet = await result.ReadFirstAsync<Timesheet>();
                    timesheet.Entries = (await result.ReadAsync<TimesheetEntry>()).ToList();

                    return timesheet;
                }
            }
        }

        public async Task<TimesheetEntry> GetTimesheetEntry(int id)
        {
            var result = await QueryAsync<TimesheetEntry>("SELECT * FROM timesheetEntry WHERE timesheetEntryId = @timesheetEntryId", new {timesheetEntryId = id});
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<TimesheetEntry>> GetTimesheetEntries(int timesheetId)
        {
            return await QueryAsync<TimesheetEntry>("SELECT * FROM timesheetEntry WHERE timesheetId = @timesheetId", new {timesheetId});
        }

        public async Task<Timesheet> CreateTimesheet(Timesheet timesheet)
        {
            const string SQL = @"INSERT INTO Timesheet (employeeId, workDate, createdDate, createdBy, modifiedDate, modifiedBy)
                                 VALUES (@employeeId, @workDate, @createdDate, @createdBy, @modifiedDate, @modifiedBy);
                                 SELECT CAST (SCOPE_IDENTITY() as int)";
            timesheet.CreatedBy = userContext.UserId;
            timesheet.ModifiedBy = userContext.UserId;
            timesheet.CreatedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            timesheet.ModifiedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            var timesheetId = await Insert<int>(SQL, new
            {
                employeeId = timesheet.EmployeeId,
                workDate = timesheet.WorkDate,
                createdBy = timesheet.CreatedBy,
                createdDate = timesheet.CreatedDate,
                modifiedBy = timesheet.ModifiedBy,
                modifiedDate = timesheet.ModifiedDate
            });
            timesheet.TimesheetId = timesheetId;

            foreach (var tsEntry in timesheet.Entries)
            {
                tsEntry.TimesheetId = timesheetId;
                var tsEntrySaved = await CreateTimesheetEntry(tsEntry);
                tsEntry.TimesheetEntryId = tsEntrySaved.TimesheetEntryId;
            }
            return timesheet;
        }

        public async Task<TimesheetEntry> CreateTimesheetEntry(TimesheetEntry timesheetEntry)
        {
            const string SQL = @"INSERT INTO TimesheetEntry (timesheetId, startDate, endDate, notes)
                                 VALUES (@timesheetId, @startDate, @endDate, @notes);
                                 SELECT CAST (SCOPE_IDENTITY() as int)";
            var tsEntryId = await Insert<int>(SQL, new
            {
                timesheetId = timesheetEntry.TimesheetId,
                startDate = timesheetEntry.StartDate,
                endDate = timesheetEntry.EndDate,
                notes = timesheetEntry.Notes
            });
            timesheetEntry.TimesheetEntryId = tsEntryId;
            return timesheetEntry;
        }
    }
}
