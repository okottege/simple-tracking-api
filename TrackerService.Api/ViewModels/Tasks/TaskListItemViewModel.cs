using System;
using System.Collections.Generic;

namespace TrackerService.Api.ViewModels.Tasks
{
    public class TaskListItemViewModel
    {
        public string TaskId { get; set; }
        public List<string> Assignees { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
