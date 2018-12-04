using System;
using System.Collections.Generic;

namespace TrackerService.Data.DataObjects
{
    public class Timesheet : AuditableEntity
    {
        public int TimesheetId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime WorkDate { get; set; }

        public IList<TimesheetEntry> Entries { get; set; } = new List<TimesheetEntry>();
    }
}
