using System;
using System.ComponentModel.DataAnnotations;

namespace TrackerService.Api.ViewModels
{
    public class TimesheetEntryViewModel
    {
        public long TimesheetEntryId { get; set; }
        [Required]
        public int? TimesheetId { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
    }
}
