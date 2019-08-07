using System;
using System.Collections.Generic;

namespace TrackerService.Api.ViewModels.Tasks
{
    public class TaskDetailsViewModel
    {
        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool? Resolved { get; set; }
        public DateTime? DueDate { get; set; }
        public string Type { get; set; }
        public List<AssignmentViewModel> Assignments { get; set; } = new List<AssignmentViewModel>();
        public List<ContextViewModel> ContextItems { get; set; } = new List<ContextViewModel>();
        public string ParentTaskId { get; set; }
    }
}
