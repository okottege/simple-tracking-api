using System;

namespace TrackerService.Data.DataObjects
{
    public class TimesheetEntry : AuditableEntity
    {
        public long TimesheetEntryId { get; set; }
        public int TimesheetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
    }
}
