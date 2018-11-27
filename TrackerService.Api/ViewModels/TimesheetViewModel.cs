using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrackerService.Api.ViewModels
{
    public class TimesheetViewModel
    {
        public int TimesheetId { get; set; }

        [Required]
        public int? EmployeeId { get; set; }

        [Required]
        public DateTime? WorkDate { get; set; }

        public IEnumerable<TimesheetEntryViewModel> Entries { get; } = new List<TimesheetEntryViewModel>();
    }
}
