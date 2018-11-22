using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Repositories
{
    internal class TimesheetSqlRepository : BaseSqlRepository, ITimesheetRepository
    {
        internal TimesheetSqlRepository(string connString) : base(connString)
        {
        }

        public async Task<IEnumerable<Timesheet>> GetTimesheets()
        {
            const string SQL_QUERY = @"SELECT * FROM Timesheet";
            return await QueryAsync<Timesheet>(SQL_QUERY, null);
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
                    timesheet.TimesheetEntries = (await result.ReadAsync<TimesheetEntry>()).ToList();

                    return timesheet;
                }
            }
        }

        public async Task<TimesheetEntry> GetTimesheetEntry(int id)
        {
            var result = await QueryAsync<TimesheetEntry>("SELECT * FROM timesheetEntry WHERE timesheetEntry = @timesheetEntryId", new {timesheetEntryId = id});
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<TimesheetEntry>> GetTimesheetEntries(int timesheetId)
        {
            return await QueryAsync<TimesheetEntry>("SELECT * FROM timesheetEntry WHERE timesheetId = @timesheetId", new {timesheetId});
        }

        public async Task<int> CreateTimesheet(Timesheet timesheet)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> CreateTimesheetEntry(TimesheetEntry timesheetEntry)
        {
            throw new System.NotImplementedException();
        }
    }
}
